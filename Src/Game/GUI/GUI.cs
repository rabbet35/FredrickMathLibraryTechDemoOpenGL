﻿using RabbetGameEngine.Models;
using RabbetGameEngine.Text;
using System.Collections.Generic;

namespace RabbetGameEngine
{
    public class GUI
    {
        private Dictionary<string, GUIComponent> components = new Dictionary<string, GUIComponent>();//all of the gui related components in this GUI, such as crosshairs, health bars, menus ect. Each component can be individually hidden, changed or removed.
        private Dictionary<string, GUITextPanel> screenTextPanels = new Dictionary<string, GUITextPanel>();
        private int preHideWindowWidth;
        private int preHideWindowHeight;
        private FontFace screenFont;
        private Texture fontTexture;
        private bool wholeScreenHidden = false;
        private uint maxCharCount;
        public string screenName = "";

        public GUI(string screenName, string textFont, uint maxCharCount = 1024)
        {
            if(!TextUtil.tryGetFont(textFont, out screenFont))
            {
                Application.error("GUIScreen " + screenName + " could not load its provided font: " + textFont + ", it will have a null font!");
            }
            if(!TextureUtil.tryGetTexture(textFont, out fontTexture))
            {
                Application.error("GUIScreen " + screenName + " could not a texture for its provided font: " + textFont + ", it will have a null font texture!");
            }
            this.screenName = screenName;
            this.maxCharCount = maxCharCount;
        }
        public void requestGUIRender()
        {
            foreach(GUIComponent comp in components.Values)
            {
                comp.requestRender();
            }
        }

        public void requestTextRender()
        {
            foreach (GUITextPanel panel in screenTextPanels.Values)
            {
                if (!panel.hidden)
                    foreach (Model mod in panel.models)
                    {
                        Renderer.requestRender(RenderType.guiText, fontTexture, mod);
                    }
            }
        }

        public void buildText()
        {
            foreach (GUITextPanel panel in screenTextPanels.Values)
            {
                panel.build();
            }
        }

        /// <summary>
        ///  Add new or change already existing gui component
        /// </summary>
        public void addGuiComponent(string name, GUIComponent component)
        {
            if(components.TryGetValue(name, out GUIComponent comp))
            {
                components.Remove(name);
            }

            components.Add(name, component);
            component.setName(name);
        }

        /// <summary>
        ///  Add new text panel, or override existing one, Do not use this to update existing panels, use updateTextPanel() instead
        /// </summary>
        public void addTextPanel(string name, GUITextPanel textPanel)
        {
            if (screenTextPanels.TryGetValue(name, out GUITextPanel panel))
            {
                screenTextPanels.Remove(name);//If theres already the text panel, override it
            }
            else
            {
                textPanel.setFont(this.screenFont);
                textPanel.build();
                screenTextPanels.Add(name, textPanel);
            }
        }

        public bool getTextPanel(string name, out GUITextPanel foundPanel)
        {
            if (screenTextPanels.TryGetValue(name, out GUITextPanel result))
            {
                foundPanel = result;
                return true;
            }
            else
            {
                Application.error("GUIScreen " + screenName + " could not find requested text panel to update: " + name);
                foundPanel = null;
                return false;
            }
        }

        public void hideWholeGUIScreen()
        {
            wholeScreenHidden = true;
            preHideWindowWidth = GameInstance.gameWindowWidth;
            preHideWindowHeight = GameInstance.gameWindowHeight;
        }
        public void unHideWholeGUIScreen()
        {
            wholeScreenHidden = false;

            if(preHideWindowWidth != GameInstance.gameWindowWidth || preHideWindowHeight != GameInstance.gameWindowHeight)
            {
                onWindowResize();
            }
        }

        public void hideComponent(string name)
        {
            if (components.TryGetValue(name, out GUIComponent comp))
            {
                comp.setHide(true);
            }
        }
        public void unHideComponent(string name)
        {
            if (components.TryGetValue(name, out GUIComponent comp))
            {
                comp.setHide(false);
            }
        }
        public void deleteComponent(string name)
        {
            if (components.TryGetValue(name, out GUIComponent comp))
            {
                components.Remove(name);
            }
        }
        public void deleteTextPanel(string name)
        {
            if (screenTextPanels.TryGetValue(name, out GUITextPanel panel))
            {
                screenTextPanels.Remove(name);
                buildText();
            }
            else
            {
                Application.warn("GUIScreen " + screenName + " Could not remove requested text panel: " +  name);
            }
        }

        public void onTick()
        {
            foreach (GUIComponent component in components.Values)
            {
                component.onTick();
            }
            requestTextRender();
            requestGUIRender();
        }

        public void onWindowResize()
        {
            if (!wholeScreenHidden)
            {
                foreach (GUIComponent component in components.Values)
                {
                    component.onWindowResize();
                }
                buildText();
            }
        }

        public bool isFontNull()
        {
            return this.screenFont == null;
        }
    }
}
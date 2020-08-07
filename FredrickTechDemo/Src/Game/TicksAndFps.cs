﻿using FredrickTechDemo.FredsMath;
using System;
using System.Diagnostics;

namespace FredrickTechDemo
{
    /*This class is responasble for providing game loop timings so that the game runs
      at a consistant rate over multiple fps rates and refresh rates.*/
    public static class TicksAndFps
    {
        private static Stopwatch stopwatch = new Stopwatch();
        private static GameInstance game;
        private static long currentTime = 0;
        private static long lastTime = 0;
        private static double timer = 0;
        private static int fps = 1;
        private static int frames;
        private static double deltaTime = 0.005D;
        private static double ticksPerSecond = 0;
        private static int ticksElapsed = 0;
        private static double timePerTick;
        private static double percentToNextTick; //a decimal value between 0 and 1 which can be used as a percentage of progress towards next tick, usefull for interpolation.

        public static void init(double tps, GameInstance game)
        {
            TicksAndFps.game = game;
            ticksPerSecond = tps;
            timePerTick = 1 / ticksPerSecond;
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;
        }

        public static void update()
        {
            /*updating FPS*/
            lastTime = currentTime;
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0F;
            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
                displayFps();
            }
            frames++;

            /*updating Ticks*/

            /*resets tick counts if atleast 1 tick time has passed. Update() must only be called after using ticksElapsed to tick the game. This way we can
	          access the accumulated tick value since the last update before it resets.*/
            if (ticksElapsed > 0)
            {
                ticksElapsed = 0;
                percentToNextTick = 0;
            }


            /*increase partialTicks by the time passed divided by time per tick. Time per tick is calulcated in constructor and is a constant.
            In other words, How many "time per ticks"'s has passed since the last update? Each time the time of one tick is passed (30 tps is 0.0333333 seconds)
            parialTicks is increased by 1.*/
            percentToNextTick += deltaTime / timePerTick;
            /*ticksElapsed is equal to the number of times the passed time has reached the full duration of one tick (at 30 ticks per second, thats 0.0333333)*/
            ticksElapsed = (int)percentToNextTick;

            /*limit ticks elapsed to Half a second's worth, so the game does not speed out of control in some laggy circumstances
              In other words, the game will only "catch up" by half a second at a time. Any hangs more than half a second will not 
              be corrected for.*/
            if (ticksElapsed > ticksPerSecond/2)
            {
                ticksElapsed = (int)ticksPerSecond / 2;
            }
        }

        /*Called every second by the TicksAndFps class to display new fps if setting is on*/
        public static void displayFps()
        {
            if (GameSettings.displayFps)
            {
                String fpsString = fps.ToString();
                String fpsPanelName = "fpsDisplay";
                if (fps < 75)
                {
                    Renderer.textRenderer2D.addNewTextPanel(fpsPanelName, fpsString, Vector2F.zero, ColourF.red);
                }
                else if (fps < 120)
                {
                    Renderer.textRenderer2D.addNewTextPanel(fpsPanelName, fpsString, Vector2F.zero, ColourF.yellow);
                }
                else
                {
                    Renderer.textRenderer2D.addNewTextPanel(fpsPanelName, fpsString, Vector2F.zero, ColourF.green);
                }
            }

        }
        public static int getTicksElapsed()
        {
            return ticksElapsed;
        }
        public static float getPercentageToNextTick()
        {
            return (float)percentToNextTick;
        }

        public static long getMiliseconds()
        {
            return stopwatch.ElapsedMilliseconds;
        }
        public static int getFps()
        {
            return fps;
        }
    }
}
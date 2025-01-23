// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Difficulty.Evaluators;

namespace osu.Game.Rulesets.Osu.Difficulty.Skills
{
    /// <summary>
    /// Represents the skill required to correctly aim at every object in the map with a uniform CircleSize and normalized distances.
    /// </summary>
    public class Aim : OsuStrainSkill
    {
        public Aim(Mod[] mods, bool withSliders)
            : base(mods)
        {
            this.withSliders = withSliders;
        }

        private readonly List<double> previousStrains = new List<double>();

        private readonly bool withSliders;

        private double currentStrain;

        private double skillMultiplier => 65;
        private double strainDecayBase => 0.15;

        private double strainIncreaseRate => 10;


        private double strainDecay(double ms) => Math.Pow(strainDecayBase, ms / 1000);

        protected override double CalculateInitialStrain(double time, DifficultyHitObject current) => currentStrain * strainDecay(time - current.Previous(0).StartTime);

        protected override double StrainValueAt(DifficultyHitObject current)
        {
            double currentDifficulty = AimEvaluator.EvaluateDifficultyOf(current, withSliders) * skillMultiplier;
            double priorDifficulty = highestPreviousStrain(current, current.DeltaTime);

            double lCurrentStrain = getStrainValueOf(currentDifficulty, priorDifficulty);
            previousStrains.Add(lCurrentStrain);


            // Strain is at most 2x the difficulty of the note.
            // We cap notes to this value to prevent jumps into easy bursts from getting too much.
            return Math.Min(currentDifficulty + currentStrain, currentDifficulty * 2);
        }
        private double getStrainValueOf(double currentDifficulty, double priorDifficulty) => (priorDifficulty * strainIncreaseRate + currentDifficulty) / (strainIncreaseRate + 1);

        private double highestPreviousStrain(DifficultyHitObject current, double time)
        {
            double hardestPreviousDifficulty = 0;
            double cumulativeDeltaTime = time;

            double timeDecay(double ms) => Math.Pow(strainDecayBase, Math.Pow(ms / 900, 7));

            for (int i = 0; i < previousStrains.Count; i++)
            {
                if (cumulativeDeltaTime > 1200)
                {
                    previousStrains.RemoveRange(0, i);
                    break;
                }

                hardestPreviousDifficulty = Math.Max(hardestPreviousDifficulty, previousStrains[^(i + 1)] * timeDecay(cumulativeDeltaTime));

                cumulativeDeltaTime += current.Previous(i).DeltaTime;
            }

            return hardestPreviousDifficulty;
        }
    }
}

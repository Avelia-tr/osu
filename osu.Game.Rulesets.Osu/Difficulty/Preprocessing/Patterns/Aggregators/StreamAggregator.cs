// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Osu.Difficulty.Preprocessing.Patterns.Aggregators
{
    /// <summary>
    /// Aggregates objets into "streams", defined as groups of notes where each
    /// of them are no more than a hit window apart from each other.
    /// </summary>
    /// This is unused and may be a bad idea, as it does not give good result.
    /// Should be deleted unless a use for it is found.
    public class StreamAggregator
    {
        private readonly double hitWindow;

        public StreamAggregator(double hitWindow)
        {
            this.hitWindow = hitWindow;
        }

        public List<DifficultyPattern> Aggregate(List<OsuDifficultyHitObject> data)
        {
            List<DifficultyPattern> result = new List<DifficultyPattern>();

            OsuDifficultyHitObject? previous = null;
            DifficultyPattern currentStream = new DifficultyPattern();
            result.Add(currentStream);

            foreach (OsuDifficultyHitObject current in data)
            {
                if (previous != null && current.StartTime - previous.StartTime > hitWindow)
                {
                    currentStream = new DifficultyPattern();
                    result.Add(currentStream);
                }

                currentStream.Children.Add(current);
                previous = current;
            }

            return result;
        }
    }
}

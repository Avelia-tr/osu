// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System.Collections.Generic;
using osu.Game.Rulesets.Osu.Difficulty.Preprocessing.Patterns.Aggregators;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Osu.Difficulty.Preprocessing.Patterns
{
    public class OsuPatternPreprocessor
    {
        private readonly RhythmAggregator rhythmAggregator;
        private readonly RepetitionAggregator repetitionAggregator;

        public OsuPatternPreprocessor(HitWindows hitWindows , double clockRate)
        {
            // Using 3ms as the hitwindow, as note timings are stored in ms.
            // Might want to consider using some sort of hit window instead
            
            rhythmAggregator = new RhythmAggregator(hitWindows.WindowFor(HitResult.Great) / clockRate);

            // streamAggregator = new StreamAggregator(hitWindows.WindowFor(HitResult.Meh));

            repetitionAggregator = new RepetitionAggregator();
        }

        public void ProcessAndAssign(List<OsuDifficultyHitObject> hitObjects)
        {
            // Group notes by their interval.
            List<FlatRhythmHitObjects> flatRhythmPatterns =
                rhythmAggregator.Group<OsuDifficultyHitObject, FlatRhythmHitObjects>(hitObjects);
            flatRhythmPatterns.ForEach(item => item.FirstHitObject.Pattern.FlatRhythmPattern = item);

            // Second rhythm pass
            List<SecondPassRhythmPattern> secondPassRhythmPatterns =
                rhythmAggregator.Group<FlatRhythmHitObjects, SecondPassRhythmPattern>(flatRhythmPatterns);
            secondPassRhythmPatterns.ForEach(item => item.FirstHitObject.Pattern.SecondPassRhythmPattern = item);

            // Third rhythm pass
            List<ThirdPassRhythmPattern> thirdPassRhythmPatterns =
                rhythmAggregator.Group<SecondPassRhythmPattern, ThirdPassRhythmPattern>(secondPassRhythmPatterns);
            thirdPassRhythmPatterns.ForEach(item => item.FirstHitObject.Pattern.ThirdPassRhythmPattern = item);

            // Within each flatRhythmPattern, group notes by colour.
            
            // List<DifficultyPattern> streams = streamAggregator.Aggregate(hitObjects);

            // List<ColourSequence> colourSequences = new();
            // foreach (var stream in streams)
            // {
            //     if (stream == null || stream.Children.Count() <= 5)
            //     {
            //         continue;
            //     }

            //     List<MonoPattern> monosInStream = colourAggregator.Group(stream.Children);
            //     colourSequences.AddRange(
            //         repetitionAggregator.Group<MonoPattern, ColourSequence>(monosInStream));
            // }
            // foreach (var item in colourSequences)
            // {
            //     item.FirstHitObject.Pattern.ColourSequence = item;
            // }

        }
    }
}

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Osu.Difficulty.Preprocessing.Patterns.Aggregators;

namespace osu.Game.Rulesets.Osu.Difficulty.Preprocessing.Patterns
{
    /// <summary>
    /// A higher order <see cref="FlatRhythm{IHasInterval}"/>, where each child are themselves <see cref="DifficultyPattern{IHasInterval}"/>s.
    /// </summary>
    public class FlatRhythmPattern<ChildrenType, InnerChildrenType> : FlatRhythm<ChildrenType>
        where ChildrenType : DifficultyPattern<InnerChildrenType>
        where InnerChildrenType : IHasInterval
    {
        public override OsuDifficultyHitObject FirstHitObject => Children[0].FirstHitObject;
    }

    /// Below are explicitly declared classes to avoid having long nested generic types.
    /// <summary>
    /// The result of aggregating <see cref="FlatRhythmHitObjects" /> by another run of <see cref="RhythmAggregator"/>.
    /// </summary>
    public class SecondPassRhythmPattern : FlatRhythmPattern<FlatRhythmHitObjects, OsuDifficultyHitObject> { }

    /// <summary>
    /// The result of aggregating <see cref="SecondPassRhythmPattern" /> by another run of <see cref="RhythmAggregator"/>.
    /// </summary>
    public class ThirdPassRhythmPattern : FlatRhythmPattern<SecondPassRhythmPattern, FlatRhythmHitObjects> { }

}

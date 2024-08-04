// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Osu.Difficulty.Preprocessing.Patterns
{
    /// <summary>
    /// A pattern that consists of a list of <typeparamref name="ChildrenType"/>. It should generally only contain
    /// consecutive objects.
    /// </summary>
    public abstract class DifficultyPattern<ChildrenType> : IHasInterval
        where ChildrenType : IHasInterval
    {
        /// <summary>
        /// The previous <see cref="DifficultyPattern{ChildrenType}"/>  />.
        /// </summary>
        public DifficultyPattern<ChildrenType>? Previous;

        public List<ChildrenType> Children { get; protected set; } = new List<ChildrenType>();

        public abstract OsuDifficultyHitObject FirstHitObject { get; }

        /// <summary>
        /// The start time of the first <see cref="DifficultyPattern{ChildrenType}.Children"/> in this <see cref="DifficultyPattern{ChildrenType}"/>.
        /// </summary>
        public double StartTime => Children[0].StartTime;

        /// <summary>
        /// The end time of the last <see cref="DifficultyPattern{ChildrenType}.Children"/> in this <see cref="DifficultyPattern{ChildrenType}"/>.
        /// </summary>
        public double EndTime => Children[^1].EndTime;

        /// <summary>
        /// The interval between <see cref="StartTime" /> of this and <see cref="EndTime" /> of the previous <see cref="DifficultyPattern{ChildrenType}"/>.
        /// </summary>
        public double Interval => Previous != null ? StartTime - Previous.EndTime : double.NaN;

        /// <summary>
        /// The duration of this <see cref="DifficultyPattern{ChildrenType}"/>.
        /// </summary>
        public double Duration => EndTime - StartTime;
    }

    /// <summary>
    /// Basic <see cref="DifficultyPattern{ChildrenType}"/> with hit objects as its children
    /// </summary>
    public class DifficultyPattern : DifficultyPattern<OsuDifficultyHitObject>
    {
        public override OsuDifficultyHitObject FirstHitObject => Children[0];
    }
}

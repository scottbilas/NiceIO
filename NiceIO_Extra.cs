// this file adds more toys to NPath without needing to modify NiceIO.cs (should help with merges from upstream)

using System;
using System.Linq;

#if NICEIO_OKTOOLS
#nullable disable
namespace OkTools.Core;
#else
namespace NiceIO;
#endif

// TODO XMLDoc :D

partial class NPath
{
    public static bool operator <(NPath left, NPath right) =>
        left is null ? right is not null : left.CompareTo(right) < 0;
    public static bool operator <=(NPath left, NPath right) =>
        left is null || left.CompareTo(right) <= 0;
    public static bool operator >(NPath left, NPath right) =>
        !(left <= right);
    public static bool operator >=(NPath left, NPath right) =>
        !(left < right);

    public static implicit operator string(NPath path) =>
        path.ToString();
    
    /// <summary>
    /// Split path at the given element index, returning two paths that, if combined, result in the original path.
    /// The subPath begins at the split index, and must be valid within the range of [0,Depth].
    /// </summary>
    public (NPath basePath, NPath subPath) SplitAtElement(int elementIndex)
    {
	    if (elementIndex < 0 || elementIndex >= Depth)
		    throw new ArgumentOutOfRangeException(nameof(elementIndex), $"Out of range 0 <= {elementIndex} < {Depth}");

	    // TODO: implement this without OldNPath
	    
	    var old = new OldNPath(this);
	    return (
		    new OldNPath(old.Elements.Take(elementIndex).ToArray(), old.IsRelative, old.DriveLetter),
		    new OldNPath(old.Elements.Skip(elementIndex).ToArray(), true, null));
    }

    public NPath ParentContaining(string needle, bool returnAppended) =>
	    ParentContaining(needle.ToNPath(), returnAppended);

    public NPath ParentContaining(NPath needle, bool returnAppended)
    {
	    var found = ParentContaining(needle);
	    if (found != null && returnAppended)
		    found = found.Combine(needle);

	    return found;
    }

    // TODO: bring over old tests also
    
    public NPath TildeExpand()
    {
	    // implementing only the most basic part of https://www.gnu.org/software/bash/manual/html_node/Tilde-Expansion.html

	    var old = new OldNPath(this);
	    
	    if (!IsRelative || old.Elements.FirstOrDefault() != "~")
		    return this;

	    return HomeDirectory.Combine(old.Elements.Skip(1).ToString());
    }

    public NPath TildeCollapse()
    {
	    var thisAbs = MakeAbsolute();
	    var homeDir = HomeDirectory;

	    if (!thisAbs.IsChildOf(HomeDirectory))
		    return this;

	    var relative = thisAbs.RelativeTo(homeDir);
	    if (relative.Depth == 0)
		    return "~";

	    return new NPath("~").Combine(relative);
    }

    // TODO: probably buggy
    public NPath ChangeFilename(string newFilename) => Parent.Combine(newFilename);
}

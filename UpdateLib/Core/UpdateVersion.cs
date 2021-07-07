using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UpdateLib.Core.Enums;

namespace UpdateLib.Core
{
    /// <summary>
    /// Versioning class with small extensions over the original <see cref="System.Version"/> as the original is sealed.
    /// Support for version label's and serializable.
    /// Partially based on Semantic Versioning <http://semver.org/>
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class UpdateVersion : IComparable, IComparable<UpdateVersion>, IEquatable<UpdateVersion>
    {
        private int m_major, m_minor, m_patch;
        private VersionLabel m_label;

        #region Constants

        private const string ALPHA_STRING = "-alpha";
        private const string BETA_STRING = "-beta";
        private const string RC_STRING = "-rc";
        private static readonly char[] CharSplitDot = new char[] { '.' };
        private static readonly char[] CharSplitDash = new char[] { '-' };
        private static readonly Regex m_regex = new Regex(@"([v]?[0-9]+){1}(\.[0-9]+){0,2}([-](alpha|beta|rc))?");

        #endregion

        #region Properties

        public int Major => m_major;

        public int Minor => m_minor;

        public int Patch => m_patch;

        public VersionLabel Label => m_label;

        public string Value
        {
            get { return ToString(); }
            set
            {
                UpdateVersion version;

                if (!TryParse(value, out version))
                    throw new ArgumentException(nameof(value), "Unable to parse input string");

                m_major = version.m_major;
                m_minor = version.m_minor;
                m_patch = version.m_patch;
                m_label = version.m_label;
            }
        }

        #endregion

        #region Constructor

        public UpdateVersion()
            : this(0, 0, 0, VersionLabel.None)
        { }

        public UpdateVersion(int major)
            : this(major, 0, 0, VersionLabel.None)
        { }

        public UpdateVersion(int major, int minor)
            : this(major, minor, 0, VersionLabel.None)
        { }

        public UpdateVersion(int major, int minor, int patch)
            : this(major, minor, patch, VersionLabel.None)
        { }

        public UpdateVersion(int major, int minor, int patch, VersionLabel label)
        {
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major), "Version cannot be negative");
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor), "Version cannot be negative");
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch), "Version cannot be negative");

            m_major = major;
            m_minor = minor;
            m_patch = patch;
            m_label = label;
        }

        public UpdateVersion(string input)
        {
            if (!TryParse(input, out UpdateVersion version))
                throw new ArgumentException(nameof(input), "Unable to parse input string");

            m_major = version.m_major;
            m_minor = version.m_minor;
            m_patch = version.m_patch;
            m_label = version.m_label;
        }

        #endregion

        #region Interface Impl.

        public int CompareTo(UpdateVersion other)
        {
            if (other == null)
                return 1;

            if (m_major != other.m_major)
                return m_major > other.m_major ? 1 : -1;

            if (m_minor != other.m_minor)
                return m_minor > other.m_minor ? 1 : -1;

            if (m_patch != other.m_patch)
                return m_patch > other.m_patch ? 1 : -1;

            if (m_label != other.m_label)
                return m_label > other.m_label ? 1 : -1;

            return 0;
        }

        public int CompareTo(object obj)
        {
            UpdateVersion other = obj as UpdateVersion;

            if (other == null)
                return 1;

            return CompareTo(other);
        }

        public bool Equals(UpdateVersion other)
        {
            if (other == null)
                return false;

            return m_major == other.m_major
                && m_minor == other.m_minor
                && m_patch == other.m_patch
                && m_label == other.m_label;
        }

        public override bool Equals(object obj)
            => Equals(obj as UpdateVersion);

        public override int GetHashCode()
        {
            int hash = 269;

            hash = (hash * 47) + Major.GetHashCode();
            hash = (hash * 47) + Minor.GetHashCode();
            hash = (hash * 47) + Patch.GetHashCode();
            hash = (hash * 47) + Label.GetHashCode();

            return hash;
        }

        #endregion

        public override string ToString() => $"{m_major}.{m_minor}.{m_patch}{LabelToString()}";

        private string LabelToString()
        {
            switch (m_label)
            {
                case VersionLabel.Alpha:
                    return ALPHA_STRING;
                case VersionLabel.Beta:
                    return BETA_STRING;
                case VersionLabel.RC:
                    return RC_STRING;
                case VersionLabel.None:
                default:
                    return string.Empty;
            }
        }

        private static bool TryParseVersionLabelString(string input, out VersionLabel label)
        {
            if (input == string.Empty)
            {
                label = VersionLabel.None;
                return true;
            }

            input = $"-{input}";

            if (input == ALPHA_STRING)
            {
                label = VersionLabel.Alpha;
                return true;
            }
            else if (input == BETA_STRING)
            {
                label = VersionLabel.Beta;
                return true;
            }
            else if (input == RC_STRING)
            {
                label = VersionLabel.RC;
                return true;
            }
            else
            {
                label = VersionLabel.None;
                return false;
            }
        }

        public static bool CanParse(string input)
            => m_regex.IsMatch(input);

        /// <summary>
        /// Tries to parse the <paramref name="input"/> to a <see cref="UpdateVersion"/>
        /// </summary>
        /// <param name="input">Input string should be of format "(v)major.minor.patch(-label)". The (v) and (-label) are optional</param>
        /// <param name="version">The output parameter</param>
        /// <returns>True if succesfully parsed, false if failed</returns>
        public static bool TryParse(string input, out UpdateVersion version)
        {
            version = new UpdateVersion();

            if (!CanParse(input)) return false;

            if (input.StartsWith("v"))
                input = input.Substring(1, input.Length - 2);

            var dashSplitTokens = input.Split(CharSplitDash);
            var tokens = dashSplitTokens[0].Split(CharSplitDot);

            if (tokens.Length > 3 || dashSplitTokens.Length > 2) // invalid version format, needs to be the following major.minor.patch(-label)
                return false;

            if (tokens.Length > 2 && !int.TryParse(tokens[2], out version.m_patch))
                return false;

            if (dashSplitTokens.Length > 1 && !TryParseVersionLabelString(dashSplitTokens[1], out version.m_label)) // unable to parse the version label
                return false;

            if (tokens.Length > 1 && !int.TryParse(tokens[1], out version.m_minor))
                return false;

            if (tokens.Length > 0 && !int.TryParse(tokens[0], out version.m_major))
                return false;

            return true; // everything parsed succesfully
        }

        public static bool operator ==(UpdateVersion v1, UpdateVersion v2)
            => ReferenceEquals(v1, null) ? ReferenceEquals(v2, null) : v1.Equals(v2);

        public static bool operator !=(UpdateVersion v1, UpdateVersion v2)
            => !(v1 == v2);

        public static bool operator >(UpdateVersion v1, UpdateVersion v2)
            => v2 < v1;

        public static bool operator >=(UpdateVersion v1, UpdateVersion v2)
            => v2 <= v1;

        public static bool operator <(UpdateVersion v1, UpdateVersion v2)
            => !ReferenceEquals(v1, null) && v1.CompareTo(v2) < 0;

        public static bool operator <=(UpdateVersion v1, UpdateVersion v2)
            => !ReferenceEquals(v1, null) && v1.CompareTo(v2) <= 0;

        public static implicit operator UpdateVersion(string value)
            => new UpdateVersion(value);

        public static implicit operator string(UpdateVersion version)
            => version.Value;
    }
}

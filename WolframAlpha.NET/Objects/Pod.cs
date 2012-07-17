using System.Collections.Generic;
using WolframAlphaNET.Objects.Errors;
using WolframAlphaNET.Objects.Output;

namespace WolframAlphaNET.Objects
{
    public class Pod
    {
        public string Title { get; set; }
        public int Position { get; set; }
        public string Scanner { get; set; }
        public string ID { get; set; }
        public bool Primary { get; set; }

        /// <summary>
        /// They only appear if the requested result formats include html.
        /// </summary>
        public Markup Markup { get; set; }
        public List<SubPod> SubPods { get; set; }
        public List<Info> Infos { get; set; }
        public StateContainer States { get; set; }
        public List<Sound> Sounds { get; set; }
        public Error Error { get; set; }

        /// <summary>
        /// Temporary link that have a lifetimes of about a half hour or so.
        /// </summary>
        public string Async { get; set; }

        public bool Equals(Pod other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ID, ID);
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Pod)) return false;
            return Equals((Pod) obj);
        }
    }
}
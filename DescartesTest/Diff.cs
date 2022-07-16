using Newtonsoft.Json.Linq;

namespace DescartesTest
{
    /// <summary>
    /// Class represents to manage Left and Right values
    /// </summary>
    public class Diff
    {
        /// <summary>
        /// Left part
        /// </summary>
        public string? Left { get; set; }

        /// <summary>
        /// Right part
        /// </summary>
        public string? Right { get; set; }

        /// <summary>
        /// Indicates whether both Left and Right are filled
        /// </summary>
        public bool IsFilled => !string.IsNullOrEmpty(Left) && !string.IsNullOrEmpty(Right);

        /// <summary>
        /// Indicates whether Left and Right are equal
        /// </summary>
        public bool IsEqual => Left == Right;

        /// <summary>
        /// Indicates whether the Length of Left and Right are same
        /// </summary>
        public bool IsEqualSize => Left?.Length == Right?.Length;

        /// <summary>
        /// Indicates whether same lenght but not same content
        /// </summary>
        public bool IsContentDoNotMatch => !IsEqual && IsEqualSize;

        /// <summary>
        /// Returns OffsetResult Array
        /// </summary>
        /// <returns></returns>
        public OffsetResult[] GetOffset()
        {
            var result = new List<OffsetResult>() {
                new OffsetResult
                {
                    Length = Left?.Length ?? 0,
                    Offset = (Left ?? "").IndexOf(Right ?? "")
                },
                new OffsetResult
                {
                    Length = Right?.Length ?? 0,
                    Offset = (Right ?? "").IndexOf(Left ?? "")
                }
            };
            return result.ToArray();
        }
    }

    public class OffsetResult
    {
        public int Offset { get; set; }
        public int Length { get; set; }
    }

    public class DiffResult
    {
        private Diff Diff { get; set; }

        public DiffResult(Diff diff)
        {
            Diff = diff;
        }

        public JObject GetResult()
        {
            var result = new JObject();

            if (Diff.IsEqual)
            {
                result = JObject.FromObject(new { diffResultType = "Equals" });
            }
            else if (!Diff.IsEqualSize)
            {
                result = JObject.FromObject(new { diffResultType = "SizeDoNotMatch" });
            }
            else if (Diff.IsContentDoNotMatch)
            {

                result = JObject.FromObject(new 
                { 
                    diffResultType = "ContentDoNotMatch",
                    diffs = Diff.GetOffset()
                });
            }
            return result;
        }

    }
}

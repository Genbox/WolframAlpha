using System;

namespace Genbox.WolframAlpha.Enums
{
    [Flags]
    public enum Format
    {
        Unknown = 0,
        Image = 1,
        Html = 2,
        Plaintext = 4,
        Minput = 8,
        Moutput = 16,
        Cell = 32,
        MathML = 64,
        ImageMap = 128,
        Sound = 256,
        Wav = 512
    }
}
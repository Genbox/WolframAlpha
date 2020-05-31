namespace Genbox.WolframAlpha.Enums
{
    public enum AssumptionType
    {
        Unknown = 0,

        /// <summary>
        /// The Clash assumption is generated when a word can represent different categories of things, such as "pi" being
        /// treated as a mathematical constant, a movie, a character, or simply as a word.
        /// </summary>
        Clash,

        /// <summary>
        /// The Unit assumption is generated when a word is interpreted as a unit abbreviation, but it is ambiguous as to
        /// what unit it represents. An example is "m", meaning either meters or minutes
        /// </summary>
        Unit,

        /// <summary>
        /// The AngleUnit assumption is generated when a number is interpreted as a unit of angle, but it is ambiguous
        /// whether it should be interpreted as degrees or radians.
        /// </summary>
        AngleUnit,

        /// <summary>
        /// The Function assumption is generated when a word is interpreted as referring to a mathematical function, but
        /// it is ambiguous which function is meant. An example is "log" meaning either log base e or log base 10.
        /// </summary>
        Function,

        /// <summary>
        /// The MultiClash assumption is a type of clash where multiple overlapping strings can have different
        /// interpretations. An example is the query "log 0.5", where the whole phrase can be interpreted as the mathematical
        /// object "log(0.5)", or the word "log" can be interpreted as a probability distribution or a plotting function
        /// </summary>
        MultiClash,

        /// <summary>
        /// The SubCategory assumption is similar to the Clash type in that a word can refer to multiple types of
        /// entities, but for SubCategory all the interpretations are within the same overall category. An example is the query
        /// "hamburger", which generates a SubCategory assumption for different types of hamburger (basic hamburger, McDonald's
        /// hamburger, Burger King hamburger, etc.) The hamburger query also generates a Clash assumption over whether hamburger
        /// should be treated as a type of food or a simple word, but given that Wolfram|Alpha is treating hamburger as a type of
        /// food in this query, it also can be resolved into subcategories of hamburger.
        /// </summary>
        SubCategory,

        /// <summary>
        /// You can think of the Attribute assumption as the next step down in the sequence of Clash and SubCategory.
        /// Wolfram|Alpha emits an Attribute assumption to allow you to modify an attribute of an already well-characterized
        /// entity. In the query "hamburger", Wolfram|Alpha assumes you mean that hamburger is a food item (although it gives you a
        /// Clash assumption to modify this) and that you mean a "basic" hamburger (and it gives you a SubCategory assumption to
        /// make this, say, a McDonald's hamburger). It also gives you an Attribute assumption to modify details like patty size
        /// and whether it has condiments.
        /// </summary>
        Attribute,

        /// <summary>
        /// When Wolfram|Alpha recognizes a string in a query as referring to a time, and it is ambiguous as to whether it
        /// represents AM or PM, a TimeAMOrPM assumption is generated.
        /// </summary>
        TimeAmOrPm,

        /// <summary>
        /// When Wolfram|Alpha recognizes a string in a query as referring to a date in numerical format, and it is
        /// ambiguous as to the order of the day, month, and year elements (such as 12/11/1996), a DateOrder assumption is
        /// generated.
        /// </summary>
        DateOrder,

        /// <summary>
        /// The ListOrTimes assumption is generated when a query contains elements separated by spaces and it is unclear
        /// whether this is to be interpreted as multiplication or a list of values. For example, the query "3 x" is interpreted as
        /// 3*x, but it could also be the list {3, x}.
        /// </summary>
        ListOrTimes,

        /// <summary>
        /// The ListOrNumber assumption is generated when a query contains a string that could be either a number with a
        /// comma as a thousands separator, or a list of two separate numbers, such as the query "1,234.5."
        /// </summary>
        ListOrNumber,

        /// <summary>
        /// The CoordinateSystem assumption is generated when it is ambiguous which coordinate system a query refers to.
        /// For example, the query "div(x rho,y z,z x)" mixes elements from standard notation for 3D Cartesian coordinates and
        /// cylindrical coordinates.
        /// </summary>
        CoordinateSystem,

        /// <summary>
        /// The I assumption is generated when a query uses "i" in a way that could refer to a simple variable name
        /// (similar to, say, "x") or the mathematical constant equal to the square root of -1.
        /// </summary>
        I,

        /// <summary>
        /// The NumberBase assumption is generated when a number could be interpreted as being written in more than one
        /// base, such as "100110101", which looks like a binary number (base 2) but could also be base 10 (it could be other bases
        /// as well, but those are rarely used and thus do not occur as assumption values).
        /// </summary>
        NumberBase,

        /// <summary>
        /// The MixedFraction assumption is generated when a string could be interpreted as either a mixed fraction or a
        /// multiplication, such as "3 1/2".
        /// </summary>
        MixedFraction,

        /// <summary>
        /// The MortalityYearDOB assumption is a very specialized type generated in some mortality-related queries, such
        /// as "life expectancy France 1910". The year 1910 could refer to the year of the data (that is, life expectancy data from
        /// France in the year 1910), or the year of birth ("life expectancy data in France for people born in 1910"). The
        /// MortalityYearDOB assumption distinguishes between those two meanings.
        /// </summary>
        MortalityYearDob,

        /// <summary>
        /// The DNAOrString assumption is generated when a query could be interpreted as a sequence of DNA bases or just a
        /// string of characters, such as "AGGGAAAA".
        /// </summary>
        DnaOrString,

        /// <summary>
        /// The TideStation assumption is generated in tide-related queries. It distinguishes between different tide
        /// stations.
        /// </summary>
        TideStation,

        FormulaSelect,
        FormulaSolve,
        FormulaVariable,
        FormulaVariableOption,
        FormulaVariableInclude,
        SelectFormulaVariable
    }
}
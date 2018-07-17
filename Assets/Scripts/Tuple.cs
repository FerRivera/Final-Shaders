
// File generated using Python 3

using System;		//For Object.Equals

public class Tuple<T1> {
	public T1	First	{ get; private set; }
	internal Tuple(T1 first)
	{
		First	= first;
	}
	public bool Equals(Tuple<T1> rhs) {
		return Object.Equals(rhs.First, First);
	}
}

public class Tuple<T1, T2> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	internal Tuple(T1 first, T2 second)
	{
		First	= first;
		Second	= second;
	}
	public bool Equals(Tuple<T1, T2> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second);
	}
}

public class Tuple<T1, T2, T3> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third)
	{
		First	= first;
		Second	= second;
		Third	= third;
	}
	public bool Equals(Tuple<T1, T2, T3> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third);
	}
}

public class Tuple<T1, T2, T3, T4> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth);
	}
}

public class Tuple<T1, T2, T3, T4, T5> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	public T15	Fifteenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
		Fifteenth	= fifteenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth) && Object.Equals(rhs.Fifteenth, Fifteenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	public T15	Fifteenth	{ get; private set; }
	public T16	Sixteenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
		Fifteenth	= fifteenth;
		Sixteenth	= sixteenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth) && Object.Equals(rhs.Fifteenth, Fifteenth) && Object.Equals(rhs.Sixteenth, Sixteenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	public T15	Fifteenth	{ get; private set; }
	public T16	Sixteenth	{ get; private set; }
	public T17	Seventeenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
		Fifteenth	= fifteenth;
		Sixteenth	= sixteenth;
		Seventeenth	= seventeenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth) && Object.Equals(rhs.Fifteenth, Fifteenth) && Object.Equals(rhs.Sixteenth, Sixteenth) && Object.Equals(rhs.Seventeenth, Seventeenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	public T15	Fifteenth	{ get; private set; }
	public T16	Sixteenth	{ get; private set; }
	public T17	Seventeenth	{ get; private set; }
	public T18	Eighteenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth, T18 eighteenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
		Fifteenth	= fifteenth;
		Sixteenth	= sixteenth;
		Seventeenth	= seventeenth;
		Eighteenth	= eighteenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth) && Object.Equals(rhs.Fifteenth, Fifteenth) && Object.Equals(rhs.Sixteenth, Sixteenth) && Object.Equals(rhs.Seventeenth, Seventeenth) && Object.Equals(rhs.Eighteenth, Eighteenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	public T15	Fifteenth	{ get; private set; }
	public T16	Sixteenth	{ get; private set; }
	public T17	Seventeenth	{ get; private set; }
	public T18	Eighteenth	{ get; private set; }
	public T19	Nineteenth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth, T18 eighteenth, T19 nineteenth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
		Fifteenth	= fifteenth;
		Sixteenth	= sixteenth;
		Seventeenth	= seventeenth;
		Eighteenth	= eighteenth;
		Nineteenth	= nineteenth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth) && Object.Equals(rhs.Fifteenth, Fifteenth) && Object.Equals(rhs.Sixteenth, Sixteenth) && Object.Equals(rhs.Seventeenth, Seventeenth) && Object.Equals(rhs.Eighteenth, Eighteenth) && Object.Equals(rhs.Nineteenth, Nineteenth);
	}
}

public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> {
	public T1	First	{ get; private set; }
	public T2	Second	{ get; private set; }
	public T3	Third	{ get; private set; }
	public T4	Fourth	{ get; private set; }
	public T5	Fifth	{ get; private set; }
	public T6	Sixth	{ get; private set; }
	public T7	Seventh	{ get; private set; }
	public T8	Eighth	{ get; private set; }
	public T9	Ninth	{ get; private set; }
	public T10	Tenth	{ get; private set; }
	public T11	Eleventh	{ get; private set; }
	public T12	Twelfth	{ get; private set; }
	public T13	Thirteenth	{ get; private set; }
	public T14	Fourteenth	{ get; private set; }
	public T15	Fifteenth	{ get; private set; }
	public T16	Sixteenth	{ get; private set; }
	public T17	Seventeenth	{ get; private set; }
	public T18	Eighteenth	{ get; private set; }
	public T19	Nineteenth	{ get; private set; }
	public T20	Twentieth	{ get; private set; }
	internal Tuple(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth, T18 eighteenth, T19 nineteenth, T20 twentieth)
	{
		First	= first;
		Second	= second;
		Third	= third;
		Fourth	= fourth;
		Fifth	= fifth;
		Sixth	= sixth;
		Seventh	= seventh;
		Eighth	= eighth;
		Ninth	= ninth;
		Tenth	= tenth;
		Eleventh	= eleventh;
		Twelfth	= twelfth;
		Thirteenth	= thirteenth;
		Fourteenth	= fourteenth;
		Fifteenth	= fifteenth;
		Sixteenth	= sixteenth;
		Seventeenth	= seventeenth;
		Eighteenth	= eighteenth;
		Nineteenth	= nineteenth;
		Twentieth	= twentieth;
	}
	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> rhs) {
		return Object.Equals(rhs.First, First) && Object.Equals(rhs.Second, Second) && Object.Equals(rhs.Third, Third) && Object.Equals(rhs.Fourth, Fourth) && Object.Equals(rhs.Fifth, Fifth) && Object.Equals(rhs.Sixth, Sixth) && Object.Equals(rhs.Seventh, Seventh) && Object.Equals(rhs.Eighth, Eighth) && Object.Equals(rhs.Ninth, Ninth) && Object.Equals(rhs.Tenth, Tenth) && Object.Equals(rhs.Eleventh, Eleventh) && Object.Equals(rhs.Twelfth, Twelfth) && Object.Equals(rhs.Thirteenth, Thirteenth) && Object.Equals(rhs.Fourteenth, Fourteenth) && Object.Equals(rhs.Fifteenth, Fifteenth) && Object.Equals(rhs.Sixteenth, Sixteenth) && Object.Equals(rhs.Seventeenth, Seventeenth) && Object.Equals(rhs.Eighteenth, Eighteenth) && Object.Equals(rhs.Nineteenth, Nineteenth) && Object.Equals(rhs.Twentieth, Twentieth);
	}
}


public static class Tuple {
	
	public static Tuple<T1> New<T1>(T1 first)
	{
		return new Tuple<T1>(first);
	}

	public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
	{
		return new Tuple<T1, T2>(first, second);
	}

	public static Tuple<T1, T2, T3> New<T1, T2, T3>(T1 first, T2 second, T3 third)
	{
		return new Tuple<T1, T2, T3>(first, second, third);
	}

	public static Tuple<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 first, T2 second, T3 third, T4 fourth)
	{
		return new Tuple<T1, T2, T3, T4>(first, second, third, fourth);
	}

	public static Tuple<T1, T2, T3, T4, T5> New<T1, T2, T3, T4, T5>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth)
	{
		return new Tuple<T1, T2, T3, T4, T5>(first, second, third, fourth, fifth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6> New<T1, T2, T3, T4, T5, T6>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6>(first, second, third, fourth, fifth, sixth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7> New<T1, T2, T3, T4, T5, T6, T7>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(first, second, third, fourth, fifth, sixth, seventh);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> New<T1, T2, T3, T4, T5, T6, T7, T8>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(first, second, third, fourth, fifth, sixth, seventh, eighth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> New<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth, seventeenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth, T18 eighteenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth, seventeenth, eighteenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth, T18 eighteenth, T19 nineteenth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth, seventeenth, eighteenth, nineteenth);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh, T8 eighth, T9 ninth, T10 tenth, T11 eleventh, T12 twelfth, T13 thirteenth, T14 fourteenth, T15 fifteenth, T16 sixteenth, T17 seventeenth, T18 eighteenth, T19 nineteenth, T20 twentieth)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth, seventeenth, eighteenth, nineteenth, twentieth);
	}

}

using System;
using System.Text;
using UnityEngine;

[SerializeField]
public class ComplexStructure
{
    public byte byteValue;
    public short shortValue;
    public int intValue;
    public long longValue;
    public float floatValue;
    public double doubleValue;
    public bool booleanValue;
    public String stringValue;

    public ComplexStructure(byte byteValue, short shortValue, int intValue, long longValue, float floatValue, double doubleValue, bool booleanValue, String stringValue)
    {
        this.byteValue = byteValue;
        this.shortValue = shortValue;
        this.intValue = intValue;
        this.longValue = longValue;
        this.floatValue = floatValue;
        this.doubleValue = doubleValue;
        this.booleanValue = booleanValue;
        this.stringValue = stringValue;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("ComplexStructure (");
        sb.Append($"byteValue={byteValue}, ");
        sb.Append($"shortValue={shortValue}, ");
        sb.Append($"intValue={intValue}, ");
        sb.Append($"longValue={longValue}, ");
        sb.Append($"floatValue={floatValue}, ");
        sb.Append($"doubleValue={doubleValue}, ");
        sb.Append($"booleanValue={booleanValue}, ");
        sb.Append($"stringValue=\"{stringValue}\"");

        sb.Append(")");
        return sb.ToString();
    }
}

package com.unity3d.player;

import org.json.JSONException;
import org.json.JSONObject;

public class ComplexStructure
{
    public byte byteValue;
    public short shortValue;
    public int intValue;
    public long longValue;
    public float floatValue;
    public double doubleValue;
    public boolean booleanValue;
    public String stringValue;

    public ComplexStructure(byte byteValue, short shortValue, int intValue, long longValue, float floatValue, double doubleValue, boolean booleanValue, String stringValue) {
        this.byteValue = byteValue;
        this.shortValue = shortValue;
        this.intValue = intValue;
        this.longValue = longValue;
        this.floatValue = floatValue;
        this.doubleValue = doubleValue;
        this.booleanValue = booleanValue;
        this.stringValue = stringValue;
    }

    public static String toJson(ComplexStructure obj) throws JSONException {
        JSONObject jsonObject = new JSONObject();
        jsonObject.put("byteValue", obj.byteValue);
        jsonObject.put("shortValue", obj.shortValue);
        jsonObject.put("intValue", obj.intValue);
        jsonObject.put("longValue", obj.longValue);
        jsonObject.put("floatValue", obj.floatValue);
        jsonObject.put("doubleValue", obj.doubleValue);
        jsonObject.put("booleanValue", obj.booleanValue);
        jsonObject.put("stringValue", obj.stringValue);

        return jsonObject.toString();
    }

    public static ComplexStructure fromJson(String json) {
        try {
            JSONObject jsonObject = new JSONObject(json);

            byte byteValue = (byte) jsonObject.getInt("byteValue");
            short shortValue = (short) jsonObject.getInt("shortValue");
            int intValue = jsonObject.getInt("intValue");
            long longValue = jsonObject.getLong("longValue");
            float floatValue = (float) jsonObject.getDouble("floatValue");
            double doubleValue = jsonObject.getDouble("doubleValue");
            boolean booleanValue = jsonObject.getBoolean("booleanValue");
            String stringValue = jsonObject.getString("stringValue");

            return new ComplexStructure(byteValue, shortValue, intValue, longValue, floatValue, doubleValue, booleanValue, stringValue);

        } catch (JSONException e) {
            System.err.println("Error parsing JSON: " + e.getMessage());
            return null;
        }
    }

    @Override
    public String toString() {
        return "ComplexStructure{" +
                "byteValue=" + byteValue +
                ", shortValue=" + shortValue +
                ", intValue=" + intValue +
                ", longValue=" + longValue +
                ", floatValue=" + floatValue +
                ", doubleValue=" + doubleValue +
                ", booleanValue=" + booleanValue +
                ", stringValue='" + stringValue + '\'' +
                '}';
    }
}
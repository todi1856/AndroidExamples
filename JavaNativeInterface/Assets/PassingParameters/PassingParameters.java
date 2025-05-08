package com.unity3d.player;

import android.util.Log;

import com.unity3d.player.ComplexStructure;

import org.json.JSONException;

public class PassingParameters
{
    public void setByte(byte value) {
        Log.v("Unity", "Java: Got byte value " + value);
    }

    public byte getByte() {
        return 122;
    }

    public void setShort(short value) {
        Log.v("Unity", "Java: Got short value " + value);
    }

    public short getShort() {
        return 555;
    }

    void setInt(int value) {
        Log.v("Unity", "Java: Got int value " + value);
    }

    int getInt() {
        return 5;
    }

    void setLong(long value) {
        Log.v("Unity", "Java: Got long value " + value);
    }

    long getLong() {
        return 7;
    }

    public void setFloat(float value) {
        Log.v("Unity", "Java: Got float value " + value);
    }

    public float getFloat() {
        return 2.72f;
    }

    public void setDouble(double value) {
        Log.v("Unity", "Java: Got double value " + value);
    }

    public double getDouble() {
        return 1.456;
    }

    public void setBoolean(boolean value) {
        Log.v("Unity", "Java: Got boolean value " + value);
    }

    public boolean getBoolean() {
        return true;
    }

    public void setChar(char value) {
        Log.v("Unity", "Java: Got char value " + value);
    }

    public char getChar() {
        return 'u';
    }

    public void setString(String value) {
        Log.v("Unity", "Java: Got String value " + value);
    }

    public String getString() {
        return "Hello Java";
    }

    public void setComplexStructure(String json) {
        var value = ComplexStructure.fromJson(json);
        Log.v("Unity", "Java: Got ComplexStructure value:");
        Log.v("Unity", " " + value.toString());
    }

    public String getComplexStructure() throws JSONException {
        var value = new ComplexStructure((byte) 10, (short) 20, 30, 40L, 50.5f, 60.6d, true, "Hello, JSON!");
        return ComplexStructure.toJson(value);
    }
}
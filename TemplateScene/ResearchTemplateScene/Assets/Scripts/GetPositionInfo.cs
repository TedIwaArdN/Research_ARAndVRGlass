using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class GetPositionInfo : MonoBehaviour {
    public string portName;
    public int baudrate;
    SerialPort serialPort;
    float rot_ini_x, rot_ini_y, rot_ini_z;
    float rot_x, rot_y, rot_z;
    float acc_x, acc_y, acc_z;
    float acc_last_x, acc_last_y, acc_last_z;
    float acc_ini_x, acc_ini_y, acc_ini_z;
    float vel_x, vel_y, vel_z;
    Vector3 initial_position;
    Rigidbody objectRigidbody;

    // Start is called before the first frame update
    void Start() {
        serialPort = new SerialPort(portName, baudrate);
        serialPort.Open();
        initial_position = new Vector3(-1.4f, 0.2f, -2.0f);
        objectRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update() {
        ReceiveData();
        UpdateRotation();
        UpdatePosition();
        StartCoroutine(WaitForDelay(0.1f));

    }

    void ReceiveData() {
        string value = serialPort.ReadLine();
        string[] datas = null;
        try {
            datas = value.Split(',');
        }
        catch(Exception e) {
            Debug.Log(e);
        }
        try {
            if (datas[0] != "" && datas[1] != "" && datas[2] != ""
                && datas[3] != "" && datas[4] != "" && datas[5] != "") {
                // change rotation and acceleration value
                try {
                    rot_x = float.Parse(datas[0]);
                    rot_y = float.Parse(datas[1]);
                    rot_z = float.Parse(datas[2]);
                    acc_x = float.Parse(datas[3]) - acc_ini_x;
                    acc_y = float.Parse(datas[4]) - acc_ini_y;
                    acc_z = float.Parse(datas[5]) - acc_ini_z;
                    acc_x = Abs(acc_x) > 0.5 ? acc_x : 0.0f;
                    acc_y = Abs(acc_y) > 0.5 ? acc_y : 0.0f;
                    acc_z = Abs(acc_z) > 0.5 ? acc_z : 0.0f;
                    Debug.Log(acc_x + " " + acc_y + " " + acc_z);
                }
                catch (Exception) {
                    Debug.Log("Change initial value");
                    ChangeInitialValue();
                }

                // clear serial information
                serialPort.BaseStream.Flush();

            }
        }
        catch (IndexOutOfRangeException) {
            Debug.Log("Change initial value");
            ChangeInitialValue();
        }
    }

    void ChangeInitialValue() {
        string value = serialPort.ReadLine();
        string[] datas = null;
        try {
            datas = value.Split(',');
        }
        catch (Exception e) {
            Debug.Log(e);
        }

        try {
            if (datas[0] != "" && datas[1] != "" && datas[2] != ""
                && datas[3] != "" && datas[4] != "" && datas[5] != "") {
                // change rotation and acceleration value
                try {
                    rot_ini_x = float.Parse(datas[0]);
                    rot_ini_y = float.Parse(datas[1]);
                    rot_ini_z = float.Parse(datas[2]);
                    acc_x = acc_ini_x = float.Parse(datas[3]);
                    acc_y = acc_ini_y = float.Parse(datas[4]);
                    acc_z = acc_ini_z = float.Parse(datas[5]);
                    transform.position = initial_position;
                    vel_x = vel_y = vel_z = 0.0f;
                }
                catch (Exception) {
                    Debug.Log("Change value failed");
                }

                // clear serial information
                serialPort.BaseStream.Flush();

            }
        }
        catch (IndexOutOfRangeException) {
            Debug.Log("Try change again");
            ChangeInitialValue();
        }
    }

    void UpdateRotation() {
        gameObject.transform.rotation = Quaternion.Euler(rot_ini_x - rot_x, rot_y - rot_ini_y, rot_ini_z - rot_z);

    }

    void UpdatePosition() {
        //objectRigidbody.AddForce(Vector3.up * 9.8f * Time.deltaTime, ForceMode.Force);
        //acc_ini_x = acc_x;
        //acc_ini_y = acc_y;
        //acc_ini_z = acc_z;
        if (acc_x == 0 || Abs(acc_x - acc_last_x) < 0.2f) vel_x = 0;
        if (acc_y == 0 || Abs(acc_y - acc_last_y) < 0.2f) vel_y = 0;
        if (acc_z == 0 || Abs(acc_z - acc_last_z) < 0.2f) vel_z = 0;
        transform.Translate((vel_x + 0.5f * acc_x * 0.1f) * Time.deltaTime, 
            -(vel_y + 0.5f * acc_y * 0.1f) * Time.deltaTime,
            (vel_z + 0.5f * acc_z * 0.1f) * Time.deltaTime);
        vel_x += acc_x * Time.deltaTime;
        vel_y += acc_y * Time.deltaTime;
        vel_z += acc_z * Time.deltaTime;
        acc_last_x = acc_x;
        acc_last_y = acc_y;
        acc_last_z = acc_z;
    }

    float Abs(float x) {
        return x > 0.0f ? x : -x;
    }

    IEnumerator WaitForDelay(float sec) {
        yield return new WaitForSeconds(sec);
    }
}

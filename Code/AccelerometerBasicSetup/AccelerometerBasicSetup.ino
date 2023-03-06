// Basic demo for accelerometer readings from Adafruit MPU6050

// ESP32 Guide: https://RandomNerdTutorials.com/esp32-mpu-6050-accelerometer-gyroscope-arduino/
// ESP8266 Guide: https://RandomNerdTutorials.com/esp8266-nodemcu-mpu-6050-accelerometer-gyroscope-arduino/
// Arduino Guide: https://RandomNerdTutorials.com/arduino-mpu-6050-accelerometer-gyroscope/

#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>

#define DELAY 100
#define pi 3.1415926

Adafruit_MPU6050 mpu;
sensors_event_t a, g, temp;
float ro_x, ro_y, ro_z;
float velo_x, velo_y, velo_z;
float dis_x, dis_y, dis_z;

void setup(void) {
  ro_x = ro_y = ro_z = 0.0;
  velo_x = velo_y = velo_z = 0.0;
  dis_x = dis_y = dis_z = 0.0;
  
  Serial.begin(9600);
  while (!Serial)
    delay(10); // will pause Zero, Leonardo, etc until serial console opens

  Wire.write(0x68);

  Serial.println();
  Serial.println("Adafruit MPU6050 test!");

  // Try to initialize!
  if (!mpu.begin()) {
    Serial.println("Failed to find MPU6050 chip");
  }

  Serial.println("MPU6050 Found!");

  mpu.setAccelerometerRange(MPU6050_RANGE_8_G);        //  2g 4g 8g 16g
  mpu.setGyroRange(MPU6050_RANGE_1000_DEG);            //  250deg/s 500deg/s 1000deg/s 2000deg/s
  mpu.setFilterBandwidth(MPU6050_BAND_5_HZ);           //  260Hz 180Hz 94Hz 44Hz 21Hz 10Hz 5Hz
  delay(100);
}

void loop() {
  /* Get new sensor events with the readings */
  mpu.getEvent(&a, &g, &temp);
  
  ro_x += g.gyro.x * DELAY / 1000.0 * 180.0 / pi;
  ro_y += g.gyro.y * DELAY / 1000.0 * 180.0 / pi;
  ro_z += g.gyro.z * DELAY / 1000.0 * 180.0 / pi;

  /* Print out the values */
//  Serial.print("Rotation X: ");
  Serial.print(ro_x);
  Serial.print(",");
//  Serial.print(", Y: ");
  Serial.print(ro_y);
  Serial.print(",");
//  Serial.print(", Z: ");
  Serial.print(ro_z);
  Serial.print(",");
//  Serial.println(" rad    ");
  
//  Serial.print("Acceleration X: ");
  Serial.print(a.acceleration.x);
  Serial.print(",");
//  Serial.print(", Y: ");
  Serial.print(a.acceleration.y);
  Serial.print(",");
//  Serial.print(", Z: ");
  Serial.print(a.acceleration.z);
  Serial.println();
//  Serial.print(" m/s^2    ");
  
//  Serial.print("Temperature: ");
//  Serial.print(temp.temperature);
//  Serial.println(" degC");

  delay(DELAY);
}

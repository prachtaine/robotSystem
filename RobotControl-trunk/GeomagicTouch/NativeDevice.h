#pragma once
#include <string>
#include <HD/hd.h>
#include <stdint.h>
using namespace std;

struct GetUpdateCallbackData
{
	HHD Handle;
	HDdouble Position[3];
	HDdouble GimbalAngles[3];
	HDint Buttons;
	HDboolean InkwellSwitch;
	HDdouble Setpoint[3];
	HDboolean ForceEnabled;
};

class NativeDevice
{
public:
	NativeDevice(string DeviceName);
	void Start();
	void Stop();
	void Update();
	void UpdateSetpoint();
	~NativeDevice();

	bool deviceInitialized;

	double SetpointX;
	double SetpointY;
	double SetpointZ;

	bool SetpointEnabled;

	double X;
	double Y;
	double Z;
	double Theta1;
	double Theta2;
	double Theta3;
	bool Button1;
	bool Button2;
	bool Button3;
	bool Button4;
	bool InkwellSwitch;


	uint8_t Buttons;
private:
	HHD handle;
	string name;
	bool servoLoopRegistered = false;
	static HDSchedulerHandle schedulerHandle;
	static int schedulerClients;
	static HDCallbackCode HDCALLBACK GetUpdateCallback(void *);
	GetUpdateCallbackData* CallbackDataObject;
	HDSchedulerHandle servoLoopHandle;
};


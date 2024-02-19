#region Assembly AscensionCSharpdll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\Simulation Systems\git\SimSysMSS\Unity\Assets\Plugins\x86_64\AscensionCSharpdll.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

// Disable Rider warnings since this is a modified decompiled file
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantCast
// ReSharper disable TooWideLocalVariableScope

namespace AscensionInput
{
    public static class TrakStarDeviceModified
    {
        public const int BIRD_ERROR_SUCCESS = 0;

        public const float scale = 2.3f;

        private static Vector3[] OriginalSensorPositions;

        private static Vector3[] OriginalSensorRotations;

        public static ConnectionStatus DeviceConnected;

        public static string ConnectionErrorMessage;

        public static Vector3[] Position;

        public static Quaternion[] Rotation;

        public static Quaternion[] RotationGrippers;

        public static Vector3[] GripperRotationValues;

        public static CSystem sys;

        public static CSensor[] pSensors;

        public static int numSensors;

        private static Quaternion[] Calibration;

        private static double[] OpenDistance;

        private static double[] ClosedDistance;

        private static Thread StartUpThread;

        public static float LoadingProgress;

        [DllImport("ATC3DG")]
        private static extern int InitializeBIRDSystem();

        [DllImport("ATC3DG")]
        private static extern int GetBIRDSystemConfiguration(ref SYSTEM_CONFIGURATION systemConfiguration);

        [DllImport("ATC3DG")]
        private static extern int GetSensorConfiguration(ushort sensorID, ref SENSOR_CONFIGURATION sensorConfiguration);

        [DllImport("ATC3DG")]
        private static extern int GetTransmitterConfiguration(ushort transmitterID, ref TRANSMITTER_CONFIGURATION transmitterConfiguration);

        [DllImport("ATC3DG")]
        private static extern int SetSystemParameter(SYSTEM_PARAMETER_TYPE parameterType, IntPtr pBuffer, int bufferSize);

        [DllImport("ATC3DG")]
        private static extern int GetAsynchronousRecord(ushort sensorID, ref DOUBLE_POSITION_ANGLES_RECORD pRecord, int recordSize);

        [DllImport("ATC3DG")]
        private static extern int CloseBIRDSystem();

        [DllImport("ATC3DG")]
        private static extern int GetSensorStatus(ushort sensorID);

        static TrakStarDeviceModified()
        {
            StartUpThread = null;
            numSensors = 0;
            ConnectionErrorMessage = "";
            DeviceConnected = ConnectionStatus.Disconnected;
        }

        public static void Disconnect()
        {
            Console.WriteLine("Killing TrakStar");
            Debug.Log("Killing TrakStar");
            if (StartUpThread != null && StartUpThread.ThreadState == ThreadState.Running)
            {
                StartUpThread.Abort();
            }

            CloseBIRDSystem();
            DeviceConnected = ConnectionStatus.Disconnected;
        }

        private static DOUBLE_MATRIX_RECORD anglesToMatrix(double a, double e, double r)
        {
            DOUBLE_MATRIX_RECORD dOUBLE_MATRIX_RECORD = new DOUBLE_MATRIX_RECORD();
            double num = r * Math.PI / 180.0;
            double num2 = e * Math.PI / 180.0;
            double num3 = a * Math.PI / 180.0;
            double num4 = Math.Cos(num);
            double num5 = Math.Sin(num);
            double num6 = Math.Cos(num2);
            double num7 = Math.Sin(num2);
            double num8 = Math.Cos(num3);
            double num9 = Math.Sin(num3);
            double num10 = num4 * num7;
            double num11 = num5 * num7;
            dOUBLE_MATRIX_RECORD.s[0, 0] = num6 * num8;
            dOUBLE_MATRIX_RECORD.s[0, 1] = num6 * num9;
            dOUBLE_MATRIX_RECORD.s[0, 2] = 0.0 - num7;
            dOUBLE_MATRIX_RECORD.s[1, 0] = num11 * num8 - num4 * num9;
            dOUBLE_MATRIX_RECORD.s[1, 1] = num11 * num9 + num4 * num8;
            dOUBLE_MATRIX_RECORD.s[1, 2] = num5 * num6;
            dOUBLE_MATRIX_RECORD.s[2, 0] = num10 * num8 + num5 * num9;
            dOUBLE_MATRIX_RECORD.s[2, 1] = num10 * num9 - num5 * num8;
            dOUBLE_MATRIX_RECORD.s[2, 2] = num4 * num6;
            return dOUBLE_MATRIX_RECORD;
        }

        private static DOUBLE_POSITION_RECORD calcOffsetPosition(DOUBLE_POSITION_RECORD pos, DOUBLE_POSITION_RECORD offset, DOUBLE_MATRIX_RECORD rotation)
        {
            DOUBLE_POSITION_RECORD result = default(DOUBLE_POSITION_RECORD);
            result.x = pos.x + offset.x * rotation.s[0, 0] + offset.y * rotation.s[1, 0] + offset.z * rotation.s[2, 0];
            result.y = pos.y + offset.x * rotation.s[0, 1] + offset.y * rotation.s[1, 1] + offset.z * rotation.s[2, 1];
            result.z = pos.z + offset.x * rotation.s[0, 2] + offset.y * rotation.s[1, 2] + offset.z * rotation.s[2, 2];
            return result;
        }

        private static DOUBLE_ALL_TIME_STAMP_Q_RECORD calcOffsetRecord(DOUBLE_ALL_TIME_STAMP_Q_RECORD original, DOUBLE_POSITION_RECORD offset, DOUBLE_ANGLES_RECORD angle_align)
        {
            double a = original.a;
            double e = original.e;
            double r = original.r;
            double x = original.x;
            double y = original.y;
            double z = original.z;
            double num = a;
            double num2 = e;
            double num3 = r;
            num += angle_align.a;
            num2 += angle_align.e;
            num3 += angle_align.r;
            DOUBLE_POSITION_RECORD pos = default(DOUBLE_POSITION_RECORD);
            pos.x = x;
            pos.y = y;
            pos.z = z;
            DOUBLE_MATRIX_RECORD rotation = anglesToMatrix(num, num2, num3);
            DOUBLE_POSITION_RECORD dOUBLE_POSITION_RECORD = calcOffsetPosition(pos, offset, rotation);
            original.x = dOUBLE_POSITION_RECORD.x;
            original.y = dOUBLE_POSITION_RECORD.y;
            original.z = dOUBLE_POSITION_RECORD.z;
            original.e = num2;
            original.r = num3;
            original.a = num;
            return original;
        }

        private static bool ErrorCodeCheck(int error)
        {
            long num = error & 0x80000000u;
            long num2 = error & 0x40000000;
            long num3 = error & 0x20000000;
            long num4 = error & 0x10000000;
            long num5 = error & 0xF0000;
            long num6 = error & 0xFFFF;
            if (num != 0 || num2 != 0)
            {
                string text = "";
                if (num != 0)
                {
                    text += "TrakError ";
                }
                else if (num2 != 0)
                {
                    text += "TrakWarning ";
                }

                text = ((num3 != 0) ? (text + "(Transmitter) ") : ((num4 == 0) ? (text + "(System) ") : (text + "(Sensor) ")));
                object obj = text;
                text = string.Concat(obj, "#", num5, " ");
                BIRD_ERROR_CODES bIRD_ERROR_CODES = (BIRD_ERROR_CODES)num6;
                text += bIRD_ERROR_CODES;
                Console.WriteLine(text);
                Debug.Log(text);
            }

            return num != 0;
        }

        public static void Connect()
        {
            LoadingProgress = 0f;
            DeviceConnected = ConnectionStatus.Connecting;
            if (StartUpThread != null && StartUpThread.ThreadState == ThreadState.Running)
            {
                Console.WriteLine("Aborting setup");
                Debug.Log("Aborting setup");
                StartUpThread.Abort();
                Disconnect();
            }

            Console.WriteLine("Starting TrakStar");
            Debug.Log("Starting TrakStar");
            StartUpThread = new Thread(SetupTrakStar);
            StartUpThread.Start(); 
        }

        public unsafe static void SetupTrakStar()
        {
            LoadingProgress = 0.1f;
            DeviceConnected = ConnectionStatus.Connecting;
            Console.WriteLine("Initing Trakstar.");
            Debug.Log("Initing Trakstar.");
            sys = new CSystem();
            Console.WriteLine("Initing Trakstar2.");
            Debug.Log("Initing Trakstar2.");
            int num = 0;
            try
            {
                num = InitializeBIRDSystem();
            }
            catch
            {
                Console.WriteLine("There was an error");
                Debug.Log("There was an error");
            }

            Console.WriteLine("BIRD Initialized.");
            Debug.Log("BIRD Initialized.");
            if (ErrorCodeCheck(num))
            {
                ConnectionErrorMessage = "Unable to initialize system";
                Console.WriteLine("ERROR: Unable to initialize the BIRD System");
                Debug.Log("ERROR: Unable to initialize the BIRD System");
                Console.WriteLine(num);
                Debug.Log(num);
                return;
            }

            num = GetBIRDSystemConfiguration(ref sys.m_config);
            if (ErrorCodeCheck(num))
            {
                ConnectionErrorMessage = "Unable to configure system";
                Console.WriteLine("ERROR: Unable to get the BIRD System Configuration");
                Debug.Log("ERROR: Unable to get the BIRD System Configuration");
                return;
            }

            pSensors = new CSensor[sys.m_config.numberSensors];
            numSensors = sys.m_config.numberSensors;
            OriginalSensorPositions = new Vector3[numSensors];
            OriginalSensorRotations = new Vector3[numSensors];
            for (int i = 0; i < numSensors; i++)
            {
                ref Vector3 reference = ref OriginalSensorPositions[i];
                reference = new Vector3(0f, 0f, 0f);
                ref Vector3 reference2 = ref OriginalSensorRotations[i];
                reference2 = new Vector3(0f, 0f, 0f);
            }

            Position = new Vector3[numSensors];
            Rotation = new Quaternion[numSensors];
            RotationGrippers = new Quaternion[numSensors];
            GripperRotationValues = new Vector3[numSensors];
            Calibration = new Quaternion[numSensors];
            int num2 = numSensors / 2 + numSensors % 2;
            if (numSensors == 0)
            {
                num2 = 1;
            }


            OpenDistance = new double[num2];
            ClosedDistance = new double[num2];
            for (int j = 0; j < num2; j++)
            {
                OpenDistance[j] = 1.5;
                ClosedDistance[j] = 0.60000002384185791;
            }


            for (int k = 0; k < numSensors; k++)
            {
                ref Quaternion reference3 = ref Calibration[k];
                reference3 = new Quaternion(1f, 0f, 0f, 0f);
            }


            int num3 = 1;
            for (ushort num4 = 0; num4 < sys.m_config.numberSensors; num4 = (ushort)(num4 + 1))
            {
                pSensors[num4] = new CSensor();
                num = GetSensorConfiguration(num4, ref pSensors[num4].m_config);
                if (ErrorCodeCheck(num))
                {
                    Console.WriteLine("ERROR: Unable to get the Sensor Configuration");
                    Debug.Log("ERROR: Unable to get the Sensor Configuration");
                    ConnectionErrorMessage = "Unable to get Sensor Configuration";
                    return;
                }

                pSensors[num4].id = num4;
                pSensors[num4].previousOutputRecord = null;
                pSensors[num4].history.Clear();
                num = pSensors[num4].setQualityParameters(0, (ushort)num3, 0, 12);
                if (ErrorCodeCheck(num))
                {
                    Console.WriteLine("ERROR: Setting Sensor quality parameters");
                    Debug.Log("ERROR: Setting Sensor quality parameters");
                    ConnectionErrorMessage = "Unable to set sensor quality parameters";
                    return;
                }

                pSensors[num4].setRecordType(DATA_FORMAT_TYPE.DOUBLE_ALL_TIME_STAMP_Q);
            }

            CXmtr[] array = new CXmtr[sys.m_config.numberTransmitters];
            for (ushort num5 = 0; num5 < sys.m_config.numberTransmitters; num5 = (ushort)(num5 + 1))
            {
                array[num5] = new CXmtr();
                num = GetTransmitterConfiguration(num5, ref array[num5].m_config);
                if (ErrorCodeCheck(num))
                {
                    Console.WriteLine("ERROR: Getting Transmitter Configuration");
                    Debug.Log("ERROR: Getting Transmitter Configuration");
                    ConnectionErrorMessage = "Unable to get transmitter configuration";
                }
                else if (array[num5].m_config.attached)
                {
                    num = SetSystemParameter(SYSTEM_PARAMETER_TYPE.SELECT_TRANSMITTER, (IntPtr)(&num5), Marshal.SizeOf((object)num5));
                    if (ErrorCodeCheck(num))
                    {
                        Console.WriteLine("ERROR: Setting Transmitter Configuration");
                        Debug.Log("ERROR: Setting Transmitter Configuration");
                        ConnectionErrorMessage = "Unable to set transmitter configuration";
                    }

                    break;
                }
            }

            LoadingProgress = 1f;
            ConnectionErrorMessage = "";
            DeviceConnected = ConnectionStatus.Ready;
            Console.WriteLine("TrakStar inited.");
            Debug.Log("TrakStar inited.");
        }

        public static void UpdateLocationTrakStar()
        {
            if (DeviceConnected != ConnectionStatus.Ready)
            {
                _ = DeviceConnected;
                return;
            }

            DOUBLE_POSITION_ANGLES_RECORD pRecord = default(DOUBLE_POSITION_ANGLES_RECORD);
            int numberSensors = sys.m_config.numberSensors;
            int num = 0;
            for (int i = 0; i < numberSensors; i++)
            {
                CSensor cSensor = pSensors[i];
                if (cSensor.m_config.attached)
                {
                    pRecord.x = OriginalSensorPositions[i].x;
                    pRecord.y = OriginalSensorPositions[i].y;
                    pRecord.z = OriginalSensorPositions[i].z;
                    pRecord.r = OriginalSensorRotations[i].x;
                    pRecord.e = OriginalSensorRotations[i].y;
                    pRecord.a = OriginalSensorRotations[i].z;
                    num = GetAsynchronousRecord((ushort)i, ref pRecord, Marshal.SizeOf((object)pRecord));
                    if (num != 0)
                    {
                        ErrorCodeCheck(num);
                    }

                    ref Vector3 reference = ref OriginalSensorRotations[i];
                    reference = new Vector3((float)pRecord.r, (float)pRecord.e, (float)pRecord.a);
                    ref Vector3 reference2 = ref OriginalSensorPositions[i];
                    reference2 = new Vector3((float)pRecord.x, (float)pRecord.y, (float)pRecord.z);
                }
            }

            UpdatePositionRotation();
        }

        private static void UpdatePositionRotation()
        {
            for (int i = 0; i < numSensors; i++)
            {
                Vector3 vector = new Vector3((0f - OriginalSensorPositions[i].y) * 2.3f, (0f - OriginalSensorPositions[i].z) * 2.3f, (0f - OriginalSensorPositions[i].x) * 2.3f);
                Position[i] = vector;
                _ = OriginalSensorRotations[i];
                Quaternion quaternion = Quaternion.Euler(0f - OriginalSensorRotations[i].y, OriginalSensorRotations[i].z - 180f, 0f - OriginalSensorRotations[i].x);
                Rotation[i] = quaternion;
                Quaternion quaternion2 = Quaternion.Euler(OriginalSensorRotations[i].y, OriginalSensorRotations[i].z, OriginalSensorRotations[i].x);
                RotationGrippers[i] = quaternion2;
                ref Vector3 reference = ref GripperRotationValues[i];
                reference = new Vector3(OriginalSensorRotations[i].y, OriginalSensorRotations[i].z, OriginalSensorRotations[i].x);
            }
        }

        public static void CalibrateSensors()
        {
            for (int i = 0; i < numSensors; i++)
            {
                ref Quaternion reference = ref Calibration[i];
                reference = new Quaternion(Rotation[i].x, Rotation[i].y, Rotation[i].z, Rotation[i].w);
            }
        }

        public static void CalibrateOpenDistance()
        {
            for (int i = 0; i < numSensors / 2; i++)
            {
                OpenDistance[i] = (Position[2 * i + 1] - Position[2 * i]).magnitude;
            }
        }

        public static void CalibrateClosedDistance()
        {
            for (int i = 0; i < numSensors / 2; i++)
            {
                ClosedDistance[i] = (Position[2 * i + 1] - Position[2 * i]).magnitude + 0.1f;
            }
        }

        public static Vector3 GetAvgPos(int forcepNum)
        {
            if (forcepNum * 2 > numSensors)
            {
                return new Vector3(0f, 0f, 0f);
            }

            return (Position[2 * forcepNum] + Position[2 * forcepNum + 1]) * 0.5f;
        }

        public static Quaternion GetAvgRot(int forcepNum)
        {
            if (forcepNum * 2 > numSensors)
            {
                return new Quaternion(1f, 0f, 0f, 0f);
            }

            Quaternion a = Rotation[2 * forcepNum] * Quaternion.Inverse(Calibration[2 * forcepNum]);
            Quaternion b = Rotation[2 * forcepNum + 1] * Quaternion.Inverse(Calibration[2 * forcepNum + 1]);
            return Quaternion.Slerp(a, b, 0.5f);
        }

        public static float GetPercentClosed(int forcepNum)
        {
            if (forcepNum * 2 > numSensors)
            {
                return 0f;
            }

            float magnitude = (Position[2 * forcepNum + 1] - Position[2 * forcepNum]).magnitude;
            return (float)((1.0 - ((double)magnitude - ClosedDistance[forcepNum]) / (OpenDistance[forcepNum] - ClosedDistance[forcepNum])) * 100.0);
        }
    }
}

package md5b1a8f0db4c0ad58f88ba03cc6ee9a4d7;


public class GattCallback
	extends android.bluetooth.BluetoothGattCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onConnectionStateChange:(Landroid/bluetooth/BluetoothGatt;II)V:GetOnConnectionStateChange_Landroid_bluetooth_BluetoothGatt_IIHandler\n" +
			"n_onServicesDiscovered:(Landroid/bluetooth/BluetoothGatt;I)V:GetOnServicesDiscovered_Landroid_bluetooth_BluetoothGatt_IHandler\n" +
			"n_onDescriptorRead:(Landroid/bluetooth/BluetoothGatt;Landroid/bluetooth/BluetoothGattDescriptor;I)V:GetOnDescriptorRead_Landroid_bluetooth_BluetoothGatt_Landroid_bluetooth_BluetoothGattDescriptor_IHandler\n" +
			"n_onCharacteristicRead:(Landroid/bluetooth/BluetoothGatt;Landroid/bluetooth/BluetoothGattCharacteristic;I)V:GetOnCharacteristicRead_Landroid_bluetooth_BluetoothGatt_Landroid_bluetooth_BluetoothGattCharacteristic_IHandler\n" +
			"n_onCharacteristicChanged:(Landroid/bluetooth/BluetoothGatt;Landroid/bluetooth/BluetoothGattCharacteristic;)V:GetOnCharacteristicChanged_Landroid_bluetooth_BluetoothGatt_Landroid_bluetooth_BluetoothGattCharacteristic_Handler\n" +
			"";
		mono.android.Runtime.register ("Robotics.Mobile.Core.Bluetooth.LE.GattCallback, Robotics.Mobile.Core.Droid", GattCallback.class, __md_methods);
	}


	public GattCallback ()
	{
		super ();
		if (getClass () == GattCallback.class)
			mono.android.TypeManager.Activate ("Robotics.Mobile.Core.Bluetooth.LE.GattCallback, Robotics.Mobile.Core.Droid", "", this, new java.lang.Object[] {  });
	}

	public GattCallback (md5b1a8f0db4c0ad58f88ba03cc6ee9a4d7.Adapter p0)
	{
		super ();
		if (getClass () == GattCallback.class)
			mono.android.TypeManager.Activate ("Robotics.Mobile.Core.Bluetooth.LE.GattCallback, Robotics.Mobile.Core.Droid", "Robotics.Mobile.Core.Bluetooth.LE.Adapter, Robotics.Mobile.Core.Droid", this, new java.lang.Object[] { p0 });
	}


	public void onConnectionStateChange (android.bluetooth.BluetoothGatt p0, int p1, int p2)
	{
		n_onConnectionStateChange (p0, p1, p2);
	}

	private native void n_onConnectionStateChange (android.bluetooth.BluetoothGatt p0, int p1, int p2);


	public void onServicesDiscovered (android.bluetooth.BluetoothGatt p0, int p1)
	{
		n_onServicesDiscovered (p0, p1);
	}

	private native void n_onServicesDiscovered (android.bluetooth.BluetoothGatt p0, int p1);


	public void onDescriptorRead (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattDescriptor p1, int p2)
	{
		n_onDescriptorRead (p0, p1, p2);
	}

	private native void n_onDescriptorRead (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattDescriptor p1, int p2);


	public void onCharacteristicRead (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1, int p2)
	{
		n_onCharacteristicRead (p0, p1, p2);
	}

	private native void n_onCharacteristicRead (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1, int p2);


	public void onCharacteristicChanged (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1)
	{
		n_onCharacteristicChanged (p0, p1);
	}

	private native void n_onCharacteristicChanged (android.bluetooth.BluetoothGatt p0, android.bluetooth.BluetoothGattCharacteristic p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
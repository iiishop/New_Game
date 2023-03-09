using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    /***
     * This class is used to send signals between objects.
     * 使用方法：
     * 1.在需要发送信号的对象中，创建一个Signal对象，例如： public static Signal signal = new Signal();
     * 2.在需要接收信号的对象中，调用Signal的AddListener方法，例如： signal.AddListener(OnSignal);
     * 3.在需要接收信号的对象中，实现OnSignal方法，例如： void OnSignal(params object[] args) { }
     * 4.在需要发送信号的对象中，调用Signal的Call方法，例如： signal.Call(args);
     */
    public delegate void SignalHandler(params object[] args);
    public event SignalHandler OnSignal;
    public void Call(params object[] info)
    {
        OnSignal?.Invoke(info);
    }
    public void AddListener(SignalHandler handler)
    {
        //保证唯一性
        OnSignal -= handler;
        OnSignal += handler;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class tic_2 : NetworkBehaviour {

	[SyncVar]
	public int turn = 1; // 表示当前回合
//	public static int turn_s = 1;

	[SyncVar]
	public SyncListInt state = new SyncListInt();
//	public static int [] map = new int[9];

  [SyncVar]
	public int result = 0;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 9; i++)                  //初始化井字棋状态
    {
        state.Add(0);
    }
		reset ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		GUI.Label(new Rect(440,40,100,50),"Tic Tac Toe");
		if (GUI.Button(new Rect(430,300,100,50),"Reset"))  //Rect()括号内表示位置，点Button就返回true
			reset();
		result = check();  // 返回0代表game没有over，1代表圈圈，2代表叉叉。
		if (result == 1) {
			if(isServer)
				GUI.Label(new Rect(450,260,100,50),"you win!");  //Label用于构建一个文本或者纹理标签
			else
				GUI.Label(new Rect(450,260,100,50),"you lost!");
		}
		else if (result == 2) {
			if(isServer)
				GUI.Label(new Rect(450,260,100,50),"you lost!");  //Label用于构建一个文本或者纹理标签
			else
				GUI.Label(new Rect(450,260,100,50),"you win!");
		}
		else
		{
			int i;
			for(i = 0;i < 9; ++i)
			{
				if(state[i] == 0) break;
			}
			if(i == 9)
				GUI.Label(new Rect(450,260,100,50),"平局!");
		}
		int k = 0;
		for (int i = 0; i < 3; ++i) {
			for (int j = 0; j < 3; ++j) {
				if (state[k] == 1)
					GUI.Button(new Rect(i * 50 + 400,j * 50 + 100,50,50),"O");
				if (state[k] == 2)
					GUI.Button(new Rect(i * 50 + 400,j * 50 +100,50,50),"X");
				if(GUI.Button(new Rect(i * 50 + 400,j * 50 + 100,50,50),"")) {
					if(isServer)
					{
						Rpctest(k,result);
						turn = 1;
					}
					else
					{
						Cmdtest(k,result);
						if (result == 0) {
							if (turn == 1)
								state[k] = 1;
							else
								state[k] = 2;
							turn = -turn;
						}
					}
				}
				k++;
			}
		}
	}

	void reset()
	{
		turn = 1;
		for (int i = 0; i < 9; ++i)
		{
				state[i] = 0;
		}
	}

	[Command]
	void Cmdtest(int k,int result){
		//Rpctest(k,result);
		if (result == 0) {
			if (turn == 1)
				state[k] = 1;
			else
				state[k] = 2;
			turn = -turn;
		}
	}

	[ClientRpc]
	void Rpctest(int k,int result)
	{
		if (result == 0) {
			if (turn == 1)
				state[k] = 1;
			else
				state[k] = 2;
			turn = -turn;
		}
	}

		int check()
		{
			for(int i = 0;i < 3; ++i)
			{
					if(state[i * 3] != 0&&state[i * 3] == state[i * 3 + 1]&&state[i * 3] == state[i * 3 + 2])
					{
						return state[i * 3];
					}
			}
			for(int i = 0;i < 3; ++i)
			{
				 if(state[i] != 0&&state[i] == state[3 + i]&&state[i] == state[6 + i])
				 {
					 return state[i];
				 }
			}
			if(state[0] != 0&&state[0] == state[4]&&state[0] == state[8])
			{
				return state[0];
			}
			if(state[2] != 0&&state[2] == state[4]&&state[2] == state[6])
			{
				return state[2];
			}
			return 0;
		}
}

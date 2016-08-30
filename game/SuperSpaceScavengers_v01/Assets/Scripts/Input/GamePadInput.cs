﻿///////////////////////////////////////////////////////////////////////////////////////////////////
//AUTHOR — Travis Moore
//SCRIPT — GamePadInput.cs
//COPYRIGHT — © 2016 DigiPen Institute of Technology
///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; // Required in C#

#region ENUMS
public enum GamePadButtonState
{ INACTIVE,
    PRESSED,
    HELD,
    RELEASED
};
#endregion

public class GamePadInput : MonoBehaviour
{
    #region FIELDS
    [Header("ENABLE/DISABLE")]
    public bool isGamePadEnabled = true;

    [Header("NUMBER OF PLAYERS")]
    [Range(1, 4)]
    [SerializeField]
    uint numberOfplayers = 1;

    [Header("DEAD ZONES")]
    [Range(0, 1)]
    [SerializeField]
    float triggerDeadZone = 0.2f;
    [Range(0, 1)]
    [SerializeField]
    float analogStickDeadZone = 0.2f;

    [Header("TIMERS")]
    public float maxButtonHeldOrReleasedTime = 60f;

    GamePadState currentState;
    GamePadState previousState;
    [HideInInspector]
    public static List<GamePadInputData> players;
    #endregion

    #region INITIALIZATION
    /// <summary>
    /// Awake()
    /// </summary>
    void Awake()
    {
    }

    /// <summary>
    /// Start()
    /// </summary>
    void Start()
    {
        //because programmers like to start with 0
        numberOfplayers -= 1;

        //create list of players
        players = new List<GamePadInputData>();
        
        //popluate list of players based on numberOfPlayers
        PopulatePlayers(numberOfplayers);

        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected and use it
        /*
        if (!playerIndexSet || !prevState.IsConnected)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)numberOfplayers;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;
                playerIndexSet = true;
            }
        }
        */
    }
    #endregion

    #region UPDATE
    /// <summary>
    /// Update()
    /// </summary>
    void Update()
    {
        //gamepad enabled? go through the update loop
        if (isGamePadEnabled)
        {
            uint _playerIndex = 0;
            //cycle through each player, check if they actually exist
            for (; _playerIndex <= numberOfplayers; ++_playerIndex)
            {
                //first test to make sure there's a controller there to update
                GamePadState _testState = GamePad.GetState((PlayerIndex)_playerIndex);
                if (_testState.IsConnected)
                {
                    //update the previous and currentstate
                    previousState = currentState;
                    currentState = GamePad.GetState((PlayerIndex)_playerIndex);

                    //check the gamepad buttons
                    CheckGamePadStates((int)_playerIndex, previousState, currentState);
                }
                else
                {
                    Debug.LogError("ERROR! Player Index for Player " + _playerIndex + " no longer exists? " + GamePad.GetState((PlayerIndex)_playerIndex));
                }
                //DEBUG — CHECK BUTTON STATUS
                //print("L3 ANALOG STICK = " + players[(int)_playerIndex].L3);
                /*
                print("L3 BUTTON IS = " + players[(int)_playerIndex].L3 + 
                        " , HELD TIMER = " + players[(int)_playerIndex].L3_HeldTimer  + 
                        " , and RELEASED TIMER = " + players[(int)_playerIndex].L3_ReleasedTimer);
                */
                //print("PLAYER " + _playerIndex + ": " + players[(int)_playerIndex].LeftAnalogStick + " and the angle is = " + players[(int)_playerIndex].LeftAnalogStickAngle);
            }
        }

    }
    #endregion

    #region METHODS
    /// <summary>
    /// Creates an array of GamePadInputData based on how many players are in the game
    /// </summary>
    /// <param name="_numberOfPlayers"># of players</param>
    void PopulatePlayers(uint _numberOfPlayers)
    {
        //cycle through the amount of players, add GamePadInputData as needed
        for (uint i = 0; i <= _numberOfPlayers; ++i)
        {
            players.Add(new GamePadInputData());
        }

        //DEBUG — HOW MANY PLAYERS ASSIGNED?
        //print("Players size = " + players.Count);
    }

    /// <summary>
    /// Checks the INACTIVE, PRESSED, HELD, and RELEASED status of buttons
    /// </summary>
    /// <param name="_previous">last frame GamePad Input</param>
    /// <param name="_current">current frame GamePad Input</param>
    void CheckGamePadStates(int _playerIndex, GamePadState _previous, GamePadState _current)
    {
        #region Y BUTTON
        //RELEASED
        if (_previous.Buttons.Y == ButtonState.Pressed && _current.Buttons.Y == ButtonState.Released)
        {
            players[_playerIndex].Y = GamePadButtonState.RELEASED;
            players[_playerIndex].Y_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.Y == ButtonState.Pressed && _current.Buttons.Y == ButtonState.Pressed)
        {
            players[_playerIndex].Y = GamePadButtonState.HELD;
            players[_playerIndex].Y_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].Y_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.Y == ButtonState.Released && _current.Buttons.Y == ButtonState.Pressed)
        {
            players[_playerIndex].Y = GamePadButtonState.PRESSED;
            players[_playerIndex].Y_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].Y = GamePadButtonState.INACTIVE;
            players[_playerIndex].Y_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].Y_ReleasedTimer);
        }
        #endregion
        #region B BUTTON
        //RELEASED
        if (_previous.Buttons.B == ButtonState.Pressed && _current.Buttons.B == ButtonState.Released)
        {
            players[_playerIndex].B = GamePadButtonState.RELEASED;
            players[_playerIndex].B_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.B == ButtonState.Pressed && _current.Buttons.B == ButtonState.Pressed)
        {
            players[_playerIndex].B = GamePadButtonState.HELD;
            players[_playerIndex].B_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].B_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.B == ButtonState.Released && _current.Buttons.B == ButtonState.Pressed)
        {
            players[_playerIndex].B = GamePadButtonState.PRESSED;
            players[_playerIndex].B_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].B = GamePadButtonState.INACTIVE;
            players[_playerIndex].B_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].B_ReleasedTimer);
        }
        #endregion
        #region A BUTTON
        //RELEASED
        if (_previous.Buttons.A == ButtonState.Pressed && _current.Buttons.A == ButtonState.Released)
        {
            players[_playerIndex].B = GamePadButtonState.RELEASED;
            players[_playerIndex].B_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.A == ButtonState.Pressed && _current.Buttons.A == ButtonState.Pressed)
        {
            players[_playerIndex].A = GamePadButtonState.HELD;
            players[_playerIndex].A_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].A_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.A == ButtonState.Released && _current.Buttons.A == ButtonState.Pressed)
        {
            players[_playerIndex].A = GamePadButtonState.PRESSED;
            players[_playerIndex].A_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].A = GamePadButtonState.INACTIVE;
            players[_playerIndex].A_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].A_ReleasedTimer);
        }
        #endregion
        #region X BUTTON
        //RELEASED
        if (_previous.Buttons.X == ButtonState.Pressed && _current.Buttons.X == ButtonState.Released)
        {
            players[_playerIndex].X = GamePadButtonState.RELEASED;
        }
        //HELD
        else if (_previous.Buttons.X == ButtonState.Pressed && _current.Buttons.X == ButtonState.Pressed)
        {
            players[_playerIndex].X = GamePadButtonState.HELD;
        }
        //PRESSED
        else if (_previous.Buttons.X == ButtonState.Released && _current.Buttons.X == ButtonState.Pressed)
        {
            players[_playerIndex].X = GamePadButtonState.PRESSED;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].X = GamePadButtonState.INACTIVE;
        }
        #endregion

        #region SELECT
        //RELEASED
        if (_previous.Buttons.Back == ButtonState.Pressed && _current.Buttons.Back == ButtonState.Released)
        {
            players[_playerIndex].Select = GamePadButtonState.RELEASED;
            players[_playerIndex].Select_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.Back == ButtonState.Pressed && _current.Buttons.Back == ButtonState.Pressed)
        {
            players[_playerIndex].Select = GamePadButtonState.HELD;
            players[_playerIndex].Select_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].Select_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.Back == ButtonState.Released && _current.Buttons.Back == ButtonState.Pressed)
        {
            players[_playerIndex].Select = GamePadButtonState.PRESSED;
            players[_playerIndex].Select_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].Select = GamePadButtonState.INACTIVE;
            players[_playerIndex].Select_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].Select_ReleasedTimer);
        }
        #endregion
        #region START
        //RELEASED
        if (_previous.Buttons.Start == ButtonState.Pressed && _current.Buttons.Start == ButtonState.Released)
        {
            players[_playerIndex].Start = GamePadButtonState.RELEASED;
            players[_playerIndex].Start_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.Start == ButtonState.Pressed && _current.Buttons.Start == ButtonState.Pressed)
        {
            players[_playerIndex].Start = GamePadButtonState.HELD;
            players[_playerIndex].Start_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].Start_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.Start == ButtonState.Released && _current.Buttons.Start == ButtonState.Pressed)
        {
            players[_playerIndex].Start = GamePadButtonState.PRESSED;
            players[_playerIndex].Start_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].Start = GamePadButtonState.INACTIVE;
            players[_playerIndex].Start_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].Start_ReleasedTimer);
        }
        #endregion

        #region LEFT SHOULDER
        //RELEASED
        if (_previous.Buttons.LeftShoulder == ButtonState.Pressed && _current.Buttons.LeftShoulder == ButtonState.Released)
        {
            players[_playerIndex].LB = GamePadButtonState.RELEASED;
            players[_playerIndex].LB_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.LeftShoulder == ButtonState.Pressed && _current.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            players[_playerIndex].LB = GamePadButtonState.HELD;
            players[_playerIndex].LB_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].LB_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.LeftShoulder == ButtonState.Released && _current.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            players[_playerIndex].LB = GamePadButtonState.PRESSED;
            players[_playerIndex].LB_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].LB = GamePadButtonState.INACTIVE;
            players[_playerIndex].LB_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].LB_ReleasedTimer);
        }
        #endregion
        #region RIGHT SHOULDER
        //RELEASED
        if (_previous.Buttons.RightShoulder == ButtonState.Pressed && _current.Buttons.RightShoulder == ButtonState.Released)
        {
            players[_playerIndex].RB = GamePadButtonState.RELEASED;
            players[_playerIndex].RB_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.RightShoulder == ButtonState.Pressed && _current.Buttons.RightShoulder == ButtonState.Pressed)
        {
            players[_playerIndex].RB = GamePadButtonState.HELD;
            players[_playerIndex].RB_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].RB_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.RightShoulder == ButtonState.Released && _current.Buttons.RightShoulder == ButtonState.Pressed)
        {
            players[_playerIndex].RB = GamePadButtonState.PRESSED;
            players[_playerIndex].RB_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].RB = GamePadButtonState.INACTIVE;
            players[_playerIndex].RB_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].RB_ReleasedTimer);
        }
        #endregion

        #region LEFT TRIGGER
        //RELEASED
        if (_previous.Triggers.Left > triggerDeadZone && _current.Triggers.Left < triggerDeadZone)
        {
            players[_playerIndex].LT = GamePadButtonState.RELEASED;
            players[_playerIndex].LTValue = _current.Triggers.Left;
            players[_playerIndex].LT_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Triggers.Left > triggerDeadZone && _current.Triggers.Left > triggerDeadZone)
        {
            players[_playerIndex].LT = GamePadButtonState.HELD;
            players[_playerIndex].LTValue = _current.Triggers.Left;
            players[_playerIndex].LT_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].LT_HeldTimer);
        }
        //PRESSED
        else if (_previous.Triggers.Left < triggerDeadZone && _current.Triggers.Left > triggerDeadZone)
        {
            players[_playerIndex].LT = GamePadButtonState.PRESSED;
            players[_playerIndex].LTValue = _current.Triggers.Left;
            players[_playerIndex].LT_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].LT = GamePadButtonState.INACTIVE;
            players[_playerIndex].LTValue = _current.Triggers.Left;
            players[_playerIndex].LT_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].LT_ReleasedTimer);
        }
        #endregion
        #region RIGHT TRIGGER
        //RELEASED
        if (_previous.Triggers.Right > triggerDeadZone && _current.Triggers.Right < triggerDeadZone)
        {
            players[_playerIndex].RT = GamePadButtonState.RELEASED;
            players[_playerIndex].RTValue = _current.Triggers.Right;
            players[_playerIndex].RT_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Triggers.Right > triggerDeadZone && _current.Triggers.Right > triggerDeadZone)
        {
            players[_playerIndex].RT = GamePadButtonState.HELD;
            players[_playerIndex].RTValue = _current.Triggers.Right;
            players[_playerIndex].RT_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].RT_HeldTimer);
        }
        //PRESSED
        else if (_previous.Triggers.Right < triggerDeadZone && _current.Triggers.Right > triggerDeadZone)
        {
            players[_playerIndex].RT = GamePadButtonState.PRESSED;
            players[_playerIndex].RTValue = _current.Triggers.Right;
            players[_playerIndex].RT_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].RT = GamePadButtonState.INACTIVE;
            players[_playerIndex].RTValue = _current.Triggers.Right;
            players[_playerIndex].RT_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].RT_ReleasedTimer);
        }
        #endregion

        #region DPAD UP
        //RELEASED
        if (_previous.DPad.Up == ButtonState.Pressed && _current.DPad.Up == ButtonState.Released)
        {
            players[_playerIndex].DPadUp = GamePadButtonState.RELEASED;
            players[_playerIndex].DPAD_UP_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.DPad.Up == ButtonState.Pressed && _current.DPad.Up == ButtonState.Pressed)
        {
            players[_playerIndex].DPadUp = GamePadButtonState.HELD;
            players[_playerIndex].DPAD_UP_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_UP_HeldTimer);
        }
        //PRESSED
        else if (_previous.DPad.Up == ButtonState.Released && _current.DPad.Up == ButtonState.Pressed)
        {
            players[_playerIndex].DPadUp = GamePadButtonState.PRESSED;
            players[_playerIndex].DPAD_UP_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].DPadUp = GamePadButtonState.INACTIVE;
            players[_playerIndex].DPAD_UP_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_UP_ReleasedTimer);
        }
        #endregion
        #region DPAD RIGHT
        //RELEASED
        if (_previous.DPad.Right == ButtonState.Pressed && _current.DPad.Right == ButtonState.Released)
        {
            players[_playerIndex].DPadRight = GamePadButtonState.RELEASED;
            players[_playerIndex].DPAD_RIGHT_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.DPad.Right == ButtonState.Pressed && _current.DPad.Right == ButtonState.Pressed)
        {
            players[_playerIndex].DPadRight = GamePadButtonState.HELD;
            players[_playerIndex].DPAD_RIGHT_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_RIGHT_HeldTimer);
        }
        //PRESSED
        else if (_previous.DPad.Right == ButtonState.Released && _current.DPad.Right == ButtonState.Pressed)
        {
            players[_playerIndex].DPadRight = GamePadButtonState.PRESSED;
            players[_playerIndex].DPAD_RIGHT_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].DPadRight = GamePadButtonState.INACTIVE;
            players[_playerIndex].DPAD_RIGHT_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_RIGHT_ReleasedTimer);
        }
        #endregion
        #region DPAD DOWN
        //RELEASED
        if (_previous.DPad.Down == ButtonState.Pressed && _current.DPad.Down == ButtonState.Released)
        {
            players[_playerIndex].DPadDown = GamePadButtonState.RELEASED;
            players[_playerIndex].DPAD_DOWN_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.DPad.Down == ButtonState.Pressed && _current.DPad.Down == ButtonState.Pressed)
        {
            players[_playerIndex].DPadDown = GamePadButtonState.HELD;
            players[_playerIndex].DPAD_DOWN_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_DOWN_HeldTimer);
        }
        //PRESSED
        else if (_previous.DPad.Down == ButtonState.Released && _current.DPad.Down == ButtonState.Pressed)
        {
            players[_playerIndex].DPadDown = GamePadButtonState.PRESSED;
            players[_playerIndex].DPAD_DOWN_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].DPadDown = GamePadButtonState.INACTIVE;
            players[_playerIndex].DPAD_DOWN_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_DOWN_ReleasedTimer);
        }
        #endregion
        #region DPAD LEFT
        //RELEASED
        if (_previous.DPad.Left == ButtonState.Pressed && _current.DPad.Left == ButtonState.Released)
        {
            players[_playerIndex].DPadLeft = GamePadButtonState.RELEASED;
            players[_playerIndex].DPAD_LEFT_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.DPad.Left == ButtonState.Pressed && _current.DPad.Left == ButtonState.Pressed)
        {
            players[_playerIndex].DPadLeft = GamePadButtonState.HELD;
            players[_playerIndex].DPAD_LEFT_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_LEFT_HeldTimer);
        }
        //PRESSED
        else if (_previous.DPad.Left == ButtonState.Released && _current.DPad.Left == ButtonState.Pressed)
        {
            players[_playerIndex].DPadLeft = GamePadButtonState.PRESSED;
            players[_playerIndex].DPAD_LEFT_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].DPadLeft = GamePadButtonState.INACTIVE;
            players[_playerIndex].DPAD_LEFT_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].DPAD_LEFT_ReleasedTimer);
        }
        #endregion

        //analog sticks !REMEMBER that angles are funky (UP is 90° RIGHT = 0°, DOWN = -90°, and LEFT = 180°)
        #region LEFT ANALOG STICK
        //respect deadzones
        if (_current.ThumbSticks.Left.X < -analogStickDeadZone ||
            _current.ThumbSticks.Left.X > analogStickDeadZone ||
            _current.ThumbSticks.Left.Y < -analogStickDeadZone ||
            _current.ThumbSticks.Left.Y > analogStickDeadZone)
        {
            //store value of x and y, along with the angle (UP is 90° RIGHT = 0°, DOWN = -90°, and LEFT = 180°)
            players[_playerIndex].LeftAnalogStick = new Vector2(_current.ThumbSticks.Left.X, _current.ThumbSticks.Left.Y);
            players[_playerIndex].LeftAnalogStickAngle = Mathf.Atan2(_current.ThumbSticks.Left.Y, _current.ThumbSticks.Left.X) * Mathf.Rad2Deg;
        }
        //RELEASED
        if (_previous.Buttons.LeftStick == ButtonState.Pressed && _current.Buttons.LeftStick == ButtonState.Released)
        {
            players[_playerIndex].L3 = GamePadButtonState.RELEASED;
            players[_playerIndex].L3_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.LeftStick == ButtonState.Pressed && _current.Buttons.LeftStick == ButtonState.Pressed)
        {
            players[_playerIndex].L3 = GamePadButtonState.HELD;
            players[_playerIndex].L3_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].L3_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.LeftStick == ButtonState.Released && _current.Buttons.LeftStick == ButtonState.Pressed)
        {
            players[_playerIndex].L3 = GamePadButtonState.PRESSED;
            players[_playerIndex].L3_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].L3 = GamePadButtonState.INACTIVE;
            players[_playerIndex].L3_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].L3_ReleasedTimer);
        }
        #endregion
        #region RIGHT ANALOG STICK
        //respect deadzones
        if (_current.ThumbSticks.Right.X < -analogStickDeadZone ||
            _current.ThumbSticks.Right.X > analogStickDeadZone ||
            _current.ThumbSticks.Right.Y < -analogStickDeadZone ||
            _current.ThumbSticks.Right.Y > analogStickDeadZone)
        {
            //store value of x and y, along with the angle (UP is 90° RIGHT = 0°, DOWN = -90°, and LEFT = 180°)
            players[_playerIndex].RightAnalogStick = new Vector2(_current.ThumbSticks.Right.X, _current.ThumbSticks.Right.Y);
            players[_playerIndex].RightAnalogStickAngle = Mathf.Atan2(_current.ThumbSticks.Right.Y, _current.ThumbSticks.Right.X) * Mathf.Rad2Deg;
        }
        //RELEASED
        if (_previous.Buttons.RightStick == ButtonState.Pressed && _current.Buttons.RightStick == ButtonState.Released)
        {
            players[_playerIndex].R3 = GamePadButtonState.RELEASED;
            players[_playerIndex].R3_HeldTimer = 0f;
        }
        //HELD
        else if (_previous.Buttons.RightStick == ButtonState.Pressed && _current.Buttons.RightStick == ButtonState.Pressed)
        {
            players[_playerIndex].R3 = GamePadButtonState.HELD;
            players[_playerIndex].R3_HeldTimer = IncrementButtonHeldTimer(players[_playerIndex].R3_HeldTimer);
        }
        //PRESSED
        else if (_previous.Buttons.RightStick == ButtonState.Released && _current.Buttons.RightStick == ButtonState.Pressed)
        {
            players[_playerIndex].R3 = GamePadButtonState.PRESSED;
            players[_playerIndex].R3_ReleasedTimer = 0f;
        }
        //INACTIVE
        else
        {
            players[_playerIndex].R3 = GamePadButtonState.INACTIVE;
            players[_playerIndex].R3_ReleasedTimer = IncrementButtonHeldTimer(players[_playerIndex].R3_ReleasedTimer);
        }
        #endregion
    }

    /// <summary>
    /// increments passed button timer by delta time
    /// </summary>
    /// <param name="_buttonTimer"> the button timer to increment</param>
    /// <returns></returns>
    float IncrementButtonHeldTimer(float _buttonTimer)
    {
        if (_buttonTimer < maxButtonHeldOrReleasedTime)
            return _buttonTimer += Time.deltaTime;
        else
            return _buttonTimer;
    }
    #endregion
}
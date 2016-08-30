﻿///////////////////////////////////////////////////////////////////////////////////////////////////
//AUTHOR — Travis Moore
//SCRIPT — ButtonBase.cs
//COPYRIGHT — © 2016 DigiPen Institute of Technology
///////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#region ENUMS
public enum MenuButtonState
{
    INACTIVE,
    HOVER,
    ACTIVE,
    DISABLED
};
#endregion

public class ButtonBase : MonoBehaviour
{
    #region FIELDS
    [HideInInspector]
    public MenuButtonState currentState = MenuButtonState.INACTIVE;

    //trs
    protected Vector3 pos
    {
        get { return transform.localPosition ; }
        set { transform.localPosition = value; }
    }
    protected Vector3 sca
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
    protected Quaternion rot
    {
        get { return transform.localRotation; }
        set { transform.localRotation = value; }
    }
    protected Vector3 pos_original
    {
        get { return transform.position; }
    }
    protected Vector3 sca_original
    {
        get { return transform.localScale; }
    }
    protected Quaternion rot_original
    {
        get { return transform.localRotation; }
    }

    //functionality
    public string levelToLoad;
    public Canvas menuToLoad;
    public float delayBeforeLoad = 2.0f;

    [Header("BUTTON")]
    public Color color_inactive = new Color(1f,1f,1f,1f);
    public Color color_hover = new Color(1f, 1f, 1f, 1f);
    public Color color_active = new Color(1f, 1f, 1f, 1f);
    public Color color_disabled = new Color(1f, 1f, 1f, 1f);
    [HideInInspector]
    public Image button;
    
    [Header("TEXT")]
    public Color textColor_inactive = new Color(0f, 0f, 0f, 1f);
    public Color textColor_hover = new Color(0f, 0f, 0f, 1f);
    public Color textColor_active = new Color(0f, 0f, 0f, 1f);
    public Color textColor_disabled = new Color(0f, 0f, 0f, 1f);
    [HideInInspector]
    public Text text;

    [Header("NODE MAP")]
    public ButtonBase up;
    public ButtonBase right;
    public ButtonBase down;
    public ButtonBase left;

    [HideInInspector]
    public bool isActive;
    #endregion

    #region INITIALIZATION
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Awake()
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    protected virtual void Awake()
    {
        //get the button and text
        button = GetComponent<Image>();
        text = transform.Find("Text").gameObject.GetComponent<Text>();

        //set trs
        sca = gameObject.transform.localScale;
        rot = gameObject.transform.localRotation;
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Start()
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    protected virtual void Start()
    {
    }
    #endregion

    #region METHODS
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// sets a button to ButtonState.INACTIVE
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void Inactive()
    {
        //update status
        currentState = MenuButtonState.INACTIVE;

        //simple change real quick
        button.color = color_inactive;
        text.color = textColor_inactive;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// sets a button to ButtonState.HOVER
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void Hover()
    {
        //update status
        currentState = MenuButtonState.HOVER;

        //simple change real quick
        button.color = color_hover;
        text.color = textColor_hover;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// sets a button to ButtonState.ACTIVE
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void Active()
    {
        //update status
        currentState = MenuButtonState.ACTIVE;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// sets a button to ButtonState.DISABLED
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void Disabled()
    {
        //update status
        currentState = MenuButtonState.DISABLED;
    }
    #endregion
}

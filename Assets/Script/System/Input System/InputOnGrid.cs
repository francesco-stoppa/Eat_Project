using System;
using UnityEngine;

public class InputOnGrid : MonoBehaviour
{
    public static InputOnGrid Instance { get; private set; }
    Controls input;
    
    // events
    public Action<E_Direction> onTakeDirection;
    public Action<E_Commands> onCommands;

    private void Awake()
    {
        // Input Map
        input = new Controls();
        if (input == null)
        {
            Debug.LogError("[Inputs] are missing.");
            return;
        }

        // Commands
        // Player direction
        input.PlayerMovement.Up.started += context => MoveUp();
        input.PlayerMovement.Down.started += context => MoveDown();
        input.PlayerMovement.Left.started += context => MoveLeft();
        input.PlayerMovement.Right.started += context => MoveRight();
        // Eat
        input.PlayerAction.EatSplit.started += context => Eat();
        // Reset level
        input.PlayerAction.Reset.started += context => ResetLevel();

        // vv singleton vv
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Movement
    void MoveUp()
    {
        onTakeDirection?.Invoke(E_Direction.Forward);
        CommandsMove();
    }
    void MoveDown()
    {
        onTakeDirection?.Invoke(E_Direction.Back);
        CommandsMove();
    }
    void MoveLeft()
    {
        onTakeDirection?.Invoke(E_Direction.Left);
        CommandsMove();
    }
    void MoveRight()
    {
        onTakeDirection?.Invoke(E_Direction.Right);
        CommandsMove();
    }
    #endregion
    #region Commands
    void CommandsMove()
    {
        onCommands?.Invoke(E_Commands.Move);
    }

    void Eat()
    {
        onCommands?.Invoke(E_Commands.EatOrSplit);
    }

    void ResetLevel()
    {
        onCommands?.Invoke(E_Commands.ResetLevel);
    }
    #endregion
    #region Enable & Disable
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    #endregion
}

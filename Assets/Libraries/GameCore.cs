using UnityEngine;

/// <summary>
/// Made by Feiko Joosten 
/// Edited by Viggy#4023 - 31/01/2019
/// Updates the Component Behaviour Classes
/// </summary>

public class GameCore : MonoBehaviour
{
    private static GameCore instance;

    private int regularUpdateArrayCount = 0;
    private int fixedUpdateArrayCount = 0;
    private int lateUpdateArrayCount = 0;
    private int regularTickArrayCount = 0;
    private int fixedTickArrayCount = 0;
    private int lateTickArrayCount = 0;
    private ComponentBehaviour[] regularArray = new ComponentBehaviour[0];
    private ComponentBehaviour[] fixedArray = new ComponentBehaviour[0];
    private ComponentBehaviour[] lateArray = new ComponentBehaviour[0];
    private ComponentBehaviour[] regularTickArray = new ComponentBehaviour[0];
    private ComponentBehaviour[] fixedTickArray = new ComponentBehaviour[0];
    private ComponentBehaviour[] lateTickArray = new ComponentBehaviour[0];

    public GameCore()
    {
        instance = this;
    }

    public static void AddItem(ComponentBehaviour behaviour)
    {
        instance.AddItemToArray(behaviour);
    }

    public static void RemoveSpecificItem(ComponentBehaviour behaviour)
    {
        instance.RemoveSpecificItemFromArray(behaviour);
    }

    public static void RemoveSpecificItemAndDestroyIt(ComponentBehaviour behaviour)
    {
        instance.RemoveSpecificItemFromArray(behaviour);

        Destroy(behaviour.gameObject);
    }

    private void AddItemToArray(ComponentBehaviour behaviour)
    {

        if (behaviour.GetType().GetMethod("OnUpdate").DeclaringType != typeof(ComponentBehaviour))
        {
            regularArray = ExtendAndAddItemToArray(regularArray, behaviour);
            regularUpdateArrayCount++;
        }

        if (behaviour.GetType().GetMethod("OnFixedUpdate").DeclaringType != typeof(ComponentBehaviour))
        {
            fixedArray = ExtendAndAddItemToArray(fixedArray, behaviour);
            fixedUpdateArrayCount++;
        }

        if (behaviour.GetType().GetMethod("OnLateUpdate").DeclaringType != typeof(ComponentBehaviour))
        {
            lateArray = ExtendAndAddItemToArray(lateArray, behaviour);
            lateUpdateArrayCount++;
        }

        if (behaviour.GetType().GetMethod("Tick").DeclaringType != typeof(ComponentBehaviour))
        {
            regularTickArray = ExtendAndAddItemToArray(regularTickArray, behaviour);
            regularTickArrayCount++;
        }

        if (behaviour.GetType().GetMethod("FixedTick").DeclaringType != typeof(ComponentBehaviour))
        {
            fixedTickArray = ExtendAndAddItemToArray(fixedTickArray, behaviour);
            fixedTickArrayCount++;
        }

        if (behaviour.GetType().GetMethod("LateTick").DeclaringType == typeof(ComponentBehaviour))
            return;

        lateTickArray = ExtendAndAddItemToArray(lateTickArray, behaviour);
        lateTickArrayCount++;

    }

    public ComponentBehaviour[] ExtendAndAddItemToArray(ComponentBehaviour[] original, ComponentBehaviour itemToAdd)
    {
        int size = original.Length;
        ComponentBehaviour[] finalArray = new ComponentBehaviour[size + 1];
        for (int i = 0; i < size; i++)
        {
            finalArray[i] = original[i];
        }
        finalArray[finalArray.Length - 1] = itemToAdd;
        return finalArray;
    }

    private void RemoveSpecificItemFromArray(ComponentBehaviour behaviour)
    {
        if (CheckIfArrayContainsItem(regularArray, behaviour))
        {
            regularArray = ShrinkAndRemoveItemToArray(regularArray, behaviour);
            regularUpdateArrayCount--;
        }

        if (CheckIfArrayContainsItem(fixedArray, behaviour))
        {
            fixedArray = ShrinkAndRemoveItemToArray(fixedArray, behaviour);
            fixedUpdateArrayCount--;
        }

        if (CheckIfArrayContainsItem(lateArray, behaviour))
        {
            lateArray = ShrinkAndRemoveItemToArray(lateArray, behaviour);
            lateUpdateArrayCount--;
        }

        if (CheckIfArrayContainsItem(regularTickArray, behaviour))
        {
            regularTickArray = ShrinkAndRemoveItemToArray(regularTickArray, behaviour);
            regularTickArrayCount--;
        }

        if (CheckIfArrayContainsItem(fixedTickArray, behaviour))
        {
            fixedTickArray = ShrinkAndRemoveItemToArray(fixedTickArray, behaviour);
            fixedTickArrayCount--;
        }

        if (!CheckIfArrayContainsItem(lateTickArray, behaviour)) return;

        lateTickArray = ShrinkAndRemoveItemToArray(lateTickArray, behaviour);
        lateTickArrayCount--;

    }

    public bool CheckIfArrayContainsItem(ComponentBehaviour[] arrayToCheck, ComponentBehaviour objectToCheckFor)
    {
        int size = arrayToCheck.Length;

        for (int i = 0; i < size; i++)
        {
            if (objectToCheckFor == arrayToCheck[i]) return true;
        }

        return false;
    }

    public ComponentBehaviour[] ShrinkAndRemoveItemToArray(ComponentBehaviour[] original, ComponentBehaviour itemToRemove)
    {
        int size = original.Length;
        ComponentBehaviour[] finalArray = new ComponentBehaviour[size - 1];
        for (int i = 0; i < size; i++)
        {
            if (original[i] == itemToRemove) continue;

            finalArray[i] = original[i];
        }
        return finalArray;
    }

    private void Update()
    {
        if (regularUpdateArrayCount > 0)
        {

            for (int i = 0; i < regularUpdateArrayCount; i++)
            {
                if (regularArray[i] == null || !regularArray[i].enabled) continue;

                regularArray[i].OnUpdate();

            }
        }

        if (regularTickArrayCount == 0) return;

        for (int i = 0; i < regularTickArrayCount; i++)
        {
            regularTickArray[i].Paused.Value = GamePaused;
            if (regularTickArray[i] == null || GamePaused || !regularTickArray[i].enabled) continue;

            regularTickArray[i].Tick();

        }
    }

    private void FixedUpdate()
    {
        if (fixedUpdateArrayCount > 0)
        {

            for (int i = 0; i < fixedUpdateArrayCount; i++)
            {
                if (fixedArray[i] == null || !fixedArray[i].enabled) continue;

                fixedArray[i].OnFixedUpdate();

            }
        }

        if (fixedTickArrayCount == 0) return;

        for (int i = 0; i < fixedTickArrayCount; i++)
        {
            fixedTickArray[i].Paused.Value = GamePaused;
            if (fixedTickArray[i] == null || GamePaused || !fixedTickArray[i].enabled) continue;

            fixedTickArray[i].FixedTick();

        }
    }

    private void LateUpdate()
    {
        if (lateUpdateArrayCount > 0)
        {

            for (int i = 0; i < lateUpdateArrayCount; i++)
            {
                if (lateArray[i] == null || !lateArray[i].enabled) continue;

                lateArray[i].OnLateUpdate();

            }
        }

        if (lateTickArrayCount == 0) return;

        for (int i = 0; i < lateTickArrayCount; i++)
        {
            lateTickArray[i].Paused.Value = GamePaused;
            if (lateTickArray[i] == null || GamePaused || !lateTickArray[i].enabled) continue;

            lateTickArray[i].LateTick();
        }
    }



    //TEST
    [SerializeField]
    private bool GamePaused = false;
}
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_CombatController : MonoBehaviour
{
    InputAction light_attack_action;
    InputAction heavy_attack_action;
    InputAction special_attack_action;

    public Animator character_anim;
    bool canAttack;
    public float attack_reset_cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light_attack_action = InputSystem.actions.FindAction("Basic");
        heavy_attack_action = InputSystem.actions.FindAction("Heavy");
        special_attack_action = InputSystem.actions.FindAction("Special");

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (light_attack_action.IsPressed() && canAttack)
        {
            canAttack = false;
            print("light attack");
            character_anim.SetTrigger("Light_Attack");
            Beat_Conductor.Instance.CheckInputAccuracy();
            Invoke(nameof(ResetAttack), attack_reset_cooldown);
        }

        if (heavy_attack_action.IsPressed() && canAttack)
        {
            canAttack = false;
            print("heavy attack");
            Beat_Conductor.Instance.CheckInputAccuracy();
            Invoke(nameof(ResetAttack), attack_reset_cooldown);
        }

        if (special_attack_action.IsPressed() && canAttack)
        {
            canAttack = false;
            print("special attack");
            Beat_Conductor.Instance.CheckInputAccuracy();
            Invoke(nameof(ResetAttack), attack_reset_cooldown);
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}

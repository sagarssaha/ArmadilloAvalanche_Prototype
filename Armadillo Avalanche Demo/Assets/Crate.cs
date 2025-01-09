using UnityEngine;

public class Crate : MonoBehaviour
{
    Rigidbody rb;
    public float pushForce;
    public ParticleSystem hitFx;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Damage_Hitbox>() != null)
        {
            hitFx.transform.position = other.transform.position;
            hitFx.Play();
            Vector3 hitDirection = this.transform.position - other.transform.parent.position;
            rb.AddForce(hitDirection.normalized * pushForce, ForceMode.Impulse);
        }
    }
}

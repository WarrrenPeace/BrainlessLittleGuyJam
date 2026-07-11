using System.Collections;
using UnityEngine;

public class Damage_VFX : MonoBehaviour
{
    private SpriteRenderer SR;
    private Material originalMat;
    [SerializeField] private Material onDamageVFXMat;
    [SerializeField] private float onDamageVFXDuration = 0.15f;
    private Coroutine onDamageVFXCo;
    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        originalMat = SR.material;
    }
    public void PlayOnDamageVFX()
    {
        if(onDamageVFXCo != null)
            StopCoroutine(OnDamageVFXCo());
            
        onDamageVFXCo = StartCoroutine(OnDamageVFXCo());
    }
    private IEnumerator OnDamageVFXCo()
    {
        SR.material = onDamageVFXMat;

        yield return new WaitForSeconds(onDamageVFXDuration);

        SR.material = originalMat;        
    }
}

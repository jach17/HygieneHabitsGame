using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particulas : MonoBehaviour
{
    [SerializeField] GameObject m_particulas;
    [SerializeField] float duracion = 2.0f;
    public void EmitirParitculas(Transform transform)
    {
        GameObject particulas = Instantiate(m_particulas, transform.position, m_particulas.transform.rotation);
        particulas.transform.SetParent(this.gameObject.transform);
        Destroy(particulas, duracion);
    }
}

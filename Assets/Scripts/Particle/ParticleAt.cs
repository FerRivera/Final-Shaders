using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ParticleAt : MonoBehaviour {

    public ParticleSystem p;
    ParticleSystem.Particle[] m_Particles;
    public int allP;
    public GameObject target;
    public float intensity;

    void Start()
    {
        //p = (ParticleSystem)(GameObject.Find("StrangeParticles").GetComponent(typeof(ParticleEmitter)));
     


    }

    void Update()
    {
        InitializeIfNeeded();

        //transform.forward = target.transform.position - transform.position;


        allP = p.GetParticles(m_Particles);
    
        for (int i = 0; i < allP; i++)
        {

            var dir = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z) - m_Particles[i].position;
            m_Particles[i].velocity += dir.normalized * Time.deltaTime * intensity;
            
            //m_Particles[i].velocity = target.transform.position - transform.position;
        }

        p.SetParticles(m_Particles, allP);

    }



    void InitializeIfNeeded()
    {
        if (p == null)
            p = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < /*p.maxParticles*/ p.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[/*p.maxParticles*/p.main.maxParticles];
    }
}

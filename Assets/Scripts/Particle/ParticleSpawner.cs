using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {

    public GameObject[] particleList;
    ParticleSystem.Particle[] _particles;
    int _partCount;
    ParticleSystem _particle;
    public int allP;
    public List<GameObject> instantiatedParticles;
    public bool gold;

    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }
    void OnParticleCollision(GameObject g)
    {



        //var main = _particle.main;
        //var collision = _particle.collision;

        // main.gravityModifier = 0;


        if (_partCount < 1)
        {
                var b = _particle.GetComponent<Renderer>().bounds;

            foreach (var particle in particleList)
            {
               
                var go = Instantiate(particle, b.center, Quaternion.identity);
                instantiatedParticles.Add(go);
            }

            var main = _particle.main;
            main.gravityModifier = 0;
            var collision = _particle.collision;
            collision.enabled = false;
            main.startSpeed = 0;
            InitializeIfNeeded();
            allP = _particle.GetParticles(_particles);
     
            for (int i = 0; i < allP; i++)
            {
                
                  _particles[i].velocity = Vector3.zero;
            }
            _particle.SetParticles(_particles, allP);



            _partCount++;

     

        }


    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == (int)LayersEnum.HERO)
        {
            if (Finder.Instance.inventory.hasSpaceEnoughItem() || gold)
            {
                foreach (var item in instantiatedParticles)
                {
                    //DestroyImmediate(item.gameObject, true);
                    Destroy(item.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }
    void InitializeIfNeeded()
    {
        if (_particle == null)
            _particle = GetComponent<ParticleSystem>();

        if (_particles == null || _particles.Length < /*p.maxParticles*/ _particle.main.maxParticles)
            _particles = new ParticleSystem.Particle[/*p.maxParticles*/_particle.main.maxParticles];
    }

}

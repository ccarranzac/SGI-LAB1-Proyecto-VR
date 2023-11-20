//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ObjectController : MonoBehaviour
{
    private bool isPointerOver = false;
    private float pointerEnterTime = 0f;
    private Vector3 originalScale;
    private Material originalMaterial;
    private Material highlightMaterial;
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    public GameObject objectToSpawn;

    public AudioClip selectionSound;

    public GameObject prefabToSpawn;

    // The objects are about 1 meter in radius, so the min/max target distance are
    // set so that the objects are always within the room (which is about 5 meters
    // across).
    private const float _minObjectDistance = 2.5f;
    private const float _maxObjectDistance = 3.5f;
    private const float _minObjectHeight = 0.5f;
    private const float _maxObjectHeight = 3.5f;

    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    GameObject instantiatedPrefab = null;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        originalMaterial = GetComponent<Renderer>().material;
        highlightMaterial = new Material(originalMaterial);
        highlightMaterial.color = Color.yellow;
        SetMaterial(false);
        GameObject specificObject = GameObject.Find("TABLE_Folding");
        specificObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Update()
    {
        if (isPointerOver && Time.time - pointerEnterTime > 2f)
        {
            var main = instantiatedPrefab.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.green);
            Debug.Log("objeto seleccionado: "+ gameObject.tag);
            AudioSource.PlayClipAtPoint(selectionSound, transform.position);
            if(gameObject.tag == "cafe"){
                // if(gameObject.name == "umbrella"){
                //     //spawn in ground
                //     Vector3 spawnPosition = transform.position - transform.right;
                //     spawnPosition.y = 0.5f;
                //     Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                // }
                // if(gameObject.name == "table.001" || gameObject.name == "table.002" || gameObject.name == "table.003" || gameObject.name == "table"){
                //     //spawn in ground
                //     Vector3 spawnPosition = transform.position;
                //     spawnPosition.y = 1f;
                //     Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                // }
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = 1f;
                spawnPosition.z = spawnPosition.z - 1.2f;
                spawnPosition.x = spawnPosition.x + 0.5f;
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            }
            if(gameObject.tag == "hotdogs"){
                Vector3 spawnPosition = transform.position + transform.right - transform.up;
                spawnPosition.y = 0.5f;
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            }
            if(gameObject.tag == "icecream"){
                Vector3 spawnPosition = transform.position - transform.right;
                spawnPosition.y = 0.5f;
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            }
            if(gameObject.tag == "stand"){
                print(gameObject.name);
                if(gameObject.name == "Side2cafe"){
                    Vector3 offset = new Vector3(0, 0, -1); // Ajusta esto a lo que necesites
                    GameObject specificObject = GameObject.Find("Player");
                    specificObject.transform.position = gameObject.transform.position + offset;
                }
                if(gameObject.name == "Side2ice"){
                    Vector3 offset = new Vector3(-2, 0, -4); // Ajusta esto a lo que necesites
                    GameObject specificObject = GameObject.Find("Player");
                    specificObject.transform.position = gameObject.transform.position + offset;
                }
                if(gameObject.name == "Side1hot"){
                    Vector3 offset = new Vector3(3, 0, 1); // Ajusta esto a lo que necesites
                    GameObject specificObject = GameObject.Find("Player");
                    specificObject.transform.position = gameObject.transform.position + offset;
                    //rotate player
                    specificObject.transform.Rotate(0, 90, 0, Space.Self);
                }
                if(gameObject.name == "Side1general"){
                    Vector3 offset = new Vector3(0, 0, 0); // Ajusta esto a lo que necesites
                    GameObject specificObject = GameObject.Find("Player");
                    specificObject.transform.position = gameObject.transform.position + offset;
                    //rotate player
                    specificObject.transform.Rotate(0, 180, 0, Space.Self);
                }
            }
            if(gameObject.tag == "table"){
                print("open menu");
                GameObject specificObject = GameObject.Find("TABLE_Folding");
                specificObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            if(gameObject.tag == "panel"){
                if(gameObject.name == "night"){
                    Camera.main.backgroundColor = Color.black;
                }
                if(gameObject.name == "day"){
                    Color32 newColor = new Color32(0x7D, 0x9A, 0xB9, 0xFF); // Replace with your hexadecimal color values
                    Camera.main.backgroundColor = newColor;
                }
                // if(gameObject.name == "close"){
                //     print("close");
                //     GameObject specificObject = GameObject.Find("TABLE_Folding");
                //     specificObject.transform.GetChild(0).gameObject.SetActive(false);
                //     if (instantiatedPrefab != null)
                //     {
                //         Destroy(instantiatedPrefab);
                //         instantiatedPrefab = null;
                //     }
                // }
            }
            isPointerOver = false;
        }
    }

    /// <summary>
    /// Teleports this instance randomly when triggered by a pointer click.
    /// </summary>
    public void TeleportRandomly()
    {
        // Picks a random sibling, activates it and deactivates itself.
        int sibIdx = transform.GetSiblingIndex();
        int numSibs = transform.parent.childCount;
        sibIdx = (sibIdx + Random.Range(1, numSibs)) % numSibs;
        GameObject randomSib = transform.parent.GetChild(sibIdx).gameObject;

        // Computes new object's location.
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float distance = Random.Range(_minObjectDistance, _maxObjectDistance);
        float height = Random.Range(_minObjectHeight, _maxObjectHeight);
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * distance, height,
                                     Mathf.Sin(angle) * distance);

        // Moves the parent to the new position (siblings relative distance from their parent is 0).
        transform.parent.localPosition = newPos;

        randomSib.SetActive(true);
        gameObject.SetActive(false);
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        if(GetComponent<Renderer>().tag != "table"){
            isPointerOver = true;
            pointerEnterTime = Time.time;
            // originalScale = transform.localScale;
            // transform.localScale += new Vector3(0.1f, 0.1f, 0.1f); // Increase size
            //print(GetComponent<Renderer>().tag != "table");
            if(GetComponent<Renderer>().name != "Cafe"){
                GetComponent<Renderer>().material = highlightMaterial;
            }
            //GetComponent<Renderer>().material = highlightMaterial;
            //SetMaterial(true);
            instantiatedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        else{
            isPointerOver = true;
            pointerEnterTime = Time.time;
            instantiatedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        if(GetComponent<Renderer>().tag != "table"){
            isPointerOver = false;
            //transform.localScale = originalScale;
            if(GetComponent<Renderer>().name != "Cafe"){
                GetComponent<Renderer>().material = originalMaterial;
            }
            //GetComponent<Renderer>().material = originalMaterial;
            //SetMaterial(false);
            if (instantiatedPrefab != null)
            {
                Destroy(instantiatedPrefab);
                instantiatedPrefab = null;
            }
        }
        else{
            isPointerOver = false;
            if (instantiatedPrefab != null)
            {
                Destroy(instantiatedPrefab);
                instantiatedPrefab = null;
            }
        }
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        TeleportRandomly();
    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}

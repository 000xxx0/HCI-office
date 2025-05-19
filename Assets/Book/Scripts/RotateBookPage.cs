using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Book
{
    public class RotateBookPage : MonoBehaviour, IDragHandler
    {
        Pages pages;

        public List<GameObject> objectsFirstPage;
        public List<GameObject> objectsLastPage;



        void Start()
        {
            pages = FindObjectOfType<Pages>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                pages.RotateForward();

            }
            else if (Input.GetAxis("Mouse X") < 0)
            {
                pages.RotateBack();
            }
        }
    }
}


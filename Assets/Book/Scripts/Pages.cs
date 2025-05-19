using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Book
{
    public class Pages : MonoBehaviour
    {
        [SerializeField] float pageSpeed = 0.5f;
        [SerializeField] List<Transform> allPages; // in this list you need add all pages you  have
        int index = -1;
        int globalIndex;
        bool rotate = false;
        GameObject newPage;
        [SerializeField] Transform pageContainer;
        Transform pag;
        bool activated = false;
        public bool rotatePage = false;



        private int GetPageIndexInAllPages(Transform pag) //Search page in the list of all pages and return index.
        {
            int index = 0;
            for (int i = 0; i < allPages.Count; i++)
            {
                if (pag.GetComponent<PageNumber>().number == allPages[i].gameObject.GetComponent<PageNumber>().number)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public void RotateForward()
        {
            if (rotate == true) { return; }

            index = 1; // index 1 means that when the page is turned it will be the last one

            pag = pageContainer.transform.GetChild(index).transform;


            CreatePageForward();
            float angle = 3.14f;

            StartCoroutine(Rotate(angle, true));

        }

        private void CreatePageForward()
        {
            globalIndex = GetPageIndexInAllPages(pag); // Search page in the list, which contains all pages and get it index
            globalIndex++;

            if (globalIndex > allPages.Count - 1)
            {
                globalIndex = 0;
            }

            newPage = Instantiate(allPages[globalIndex]).gameObject; //Create new page under the last page
            newPage.transform.parent = this.transform.parent;
            newPage.transform.SetAsFirstSibling();
            newPage.transform.position = pageContainer.transform.GetChild(0).transform.position;
            newPage.transform.localScale = Vector3.one;

            ActiveChildren(newPage.transform, true);  // active images, when the page is last


        }

        public void RotateBack()
        {
            if (rotate == true) { return; }

            index = 0; //index 0 means that when the page is turned it will be the first one

            pag = pageContainer.transform.GetChild(index).transform;

            pag.SetAsLastSibling();

            CreatePageBackward();

            float angle = 0f; //in order to rotate the page back, you need to set the rotation to 0 degrees around the y axis

            StartCoroutine(Rotate(angle, false));
        }


        private void CreatePageBackward()
        {

            globalIndex = GetPageIndexInAllPages(pag);
            globalIndex--;
            if (globalIndex < 0) // take the last page from the list of all pages
            {
                globalIndex = allPages.Count - 1;
            }
            newPage = Instantiate(allPages[globalIndex]).gameObject;

            newPage.transform.rotation = Quaternion.EulerAngles(0, 3.14f, 0);
            newPage.transform.parent = this.transform.parent;
            newPage.transform.SetAsFirstSibling();
            newPage.transform.position = pageContainer.transform.GetChild(1).transform.position;
            newPage.transform.localScale = Vector3.one;

            ActiveChildren(newPage.transform, false); // active images, when the page is first

        }





        IEnumerator Rotate(float angle, bool forward)
        {
            float value = 0f;

            Quaternion startRot = pag.rotation;

            while (true)
            {
                rotate = true;
                Transform currentPage = pag;
                Quaternion targetRotation = Quaternion.EulerAngles(0, angle, 0);
                value += Time.deltaTime * pageSpeed;
                currentPage.rotation = Quaternion.Slerp(startRot, targetRotation, value); //smoothly turn the page
                float angle1 = Quaternion.Angle(currentPage.rotation, targetRotation); //calculate the angle between the given angle of rotation and the current angle of rotation

                PageImages(value, currentPage);

                if (angle1 < 0.1f)
                {
                    PageRotated(forward);

                    rotate = false;
                    break;

                }
                yield return null;

            }

            activated = false;
        }

        private void PageRotated(bool forward)
        {
            if (forward == false)
            {
                newPage.transform.parent = pageContainer; //Add the page as child of pageContainer gameObject
                Destroy(pageContainer.transform.GetChild(0).gameObject);
                newPage.transform.SetAsFirstSibling();

            }
            else
            {
                newPage.transform.parent = pageContainer;
                Destroy(pageContainer.transform.GetChild(0).gameObject);

            }
        }

        private void PageImages(float value, Transform currentPage)
        {
            if (value > 0.5f & activated == false) // if the page rotation is approximately 90 degrees then we activate child elements depending on whether the page is first or last
            {
                if (index == 1)
                {
                    ActiveChildren(currentPage.transform, false);

                }
                else
                {
                    ActiveChildren(currentPage.transform, true);

                }
                activated = true;
            }
        }

        public void ActiveChildren(Transform transform, bool shouldActive)
        {
            RotateBookPage page = transform.GetComponentInChildren<RotateBookPage>();
            List<GameObject> objectsLastPage = page.objectsLastPage;
            List<GameObject> objectsFirstPage = page.objectsFirstPage;


            for (int i = 0; i < objectsLastPage.Count; i++)
            {
                if (objectsLastPage[i] != null)
                {
                    objectsLastPage[i].SetActive(shouldActive);
                }
            }
            for (int i = 0; i < objectsFirstPage.Count; i++)
            {
                if (objectsFirstPage[i] != null)
                {
                    objectsFirstPage[i].SetActive(!shouldActive);
                }
            }
        }


    }
}

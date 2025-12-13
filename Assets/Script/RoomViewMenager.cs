using UnityEngine;

public class RoomViewManager : MonoBehaviour
{
    public GameObject[] views;
    private int currentIndex = 0;

    void Start()
    {
        ShowView(currentIndex);
    }

    public void NextView()
    {
        currentIndex++;
        if (currentIndex >= views.Length)
            currentIndex = 0;

        ShowView(currentIndex);
    }

    public void PreviousView()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = views.Length - 1;

        ShowView(currentIndex);
    }

    private void ShowView(int index)
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetActive(i == index);
        }
    }
}

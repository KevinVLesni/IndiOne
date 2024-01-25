using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public GameObject crosshair;  // Перетащите сюда объект прицела
    private bool isMouseButtonDown = false;

    void Update()
    {
        // Обработка нажатия и отпускания кнопки мыши
        if (Input.GetMouseButtonDown(1))
        {
            isMouseButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isMouseButtonDown = false;
            ToggleCrosshairVisibility(false);
        }

        // Обновление положения прицела
        if (isMouseButtonDown)
        {
            UpdateCrosshairPosition();
        }
    }

    void UpdateCrosshairPosition()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0f;  // Устанавливаем z в 0, чтобы оставаться в 2D-пространстве

        // Перемещаем прицел в позицию курсора
        crosshair.transform.position = cursorPosition;

        // Показываем прицел
        ToggleCrosshairVisibility(true);
    }

    void ToggleCrosshairVisibility(bool isVisible)
    {
        crosshair.SetActive(isVisible);
    }
}
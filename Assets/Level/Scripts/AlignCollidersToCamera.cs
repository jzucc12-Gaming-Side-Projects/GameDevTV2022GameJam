using UnityEngine;

/// <summary>
/// Sets box colliders to border a polygon collider
/// Useful for setting player boundaries in relation to the camera boundaries
/// </summary>
public class AlignCollidersToCamera : MonoBehaviour
{
    #region //Cached Components
    [Header("Colliders")]
    [SerializeField] private BoxCollider2D leftBoxCollider = null;
    [SerializeField] private BoxCollider2D rightBoxCollider = null;
    [SerializeField] private BoxCollider2D topBoxCollider = null;
    [SerializeField] private BoxCollider2D bottomBoxCollider = null;
    #endregion

    #region //Offset
    [Header("Size and Positioning")]
    [Tooltip("Distance from polygon collider")]
    [SerializeField] private float offset = 0f;
    [SerializeField] private float boundaryWidth = 1f;
    [Tooltip("Amount boundary exceeds the polygon collider per side")]
    [SerializeField] private float overflow = 0f;
    #endregion

    
    #region //Monobehaviour
    private void OnValidate()
    {
        Start();
    }

    private void Start()
    {
        PositionPlayerBounds();
    }
    #endregion

    #region //Position player bounds around camera bounds
    [ContextMenu("Align")]
    public void PositionPlayerBounds()
    {
        if(Camera.main == null) return;
        SetupBoundary(leftBoxCollider, true, false);
        SetupBoundary(rightBoxCollider, true, true);
        SetupBoundary(topBoxCollider, false, true);
        SetupBoundary(bottomBoxCollider, false, false);
    }

    private void SetupBoundary(BoxCollider2D collider, bool isHorizontal, bool topOrRight)
    {
        //Get camera sizing
        float halfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        float halfHeight = halfWidth / Camera.main.aspect;

        //Determine indices to use based off boundary direction
        int mainIndex = isHorizontal ? 0 : 1;
        int offIndex = 1 - mainIndex;
        
        //Resize bound
        Vector2 newSize = Vector2.zero;
        newSize[mainIndex] = boundaryWidth;
        newSize[offIndex] = (isHorizontal ? halfHeight : halfWidth) * 2 + 2 * overflow;
        collider.size = newSize;

        //Determine main axis positioning
        float halfSize = newSize[mainIndex]/2;
        float mainAxisOffset = halfSize + offset;
        
        //Reposition bound
        Vector2 newPosition = Vector2.zero;
        newPosition[mainIndex] = ((isHorizontal ? halfWidth : halfHeight) + mainAxisOffset) * (topOrRight ? 1 : -1);
        newPosition[offIndex] = 0;
        collider.transform.localPosition = newPosition;
    }
    #endregion
}
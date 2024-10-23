using Unity.VisualScripting;
using UnityEngine;

public class Square
{
    private readonly SpriteRenderer _renderer;
    
    private bool _isActivePrev;
    private bool _isActiveCurrent;
    private bool _isUpdated;
    
    private static Color _activateColor;
    private static Color _deactivateColor;
    private static Color _onTargetColor;
    public Square(GameObject gameObject)
    {
        _isActivePrev = false;
        _isActiveCurrent = false;
        
        _activateColor = Color.white;
        _deactivateColor = Color.white;
        _onTargetColor = Color.white;

        _activateColor.a = 1f;
        _deactivateColor.a = 0f;
        _onTargetColor.a = .5f;
        
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.color = _deactivateColor;
        _renderer.enabled = true;
    }
    public void Activate()
    {
        _isActiveCurrent = true;
    }
    public void Deactivate()
    {
        _isActiveCurrent = false;
    }
    public void Target()
    {
        _renderer.color = _onTargetColor;
    }
    public void Untarget()
    {
        _renderer.color = _isActiveCurrent ? _activateColor : _deactivateColor;
    }
    public bool IsActive()
    {
        return _isActivePrev;
    }
    public void Update()
    {
        _isActivePrev = _isActiveCurrent;
        _renderer.color = _isActiveCurrent ? _activateColor : _deactivateColor;
    }
}

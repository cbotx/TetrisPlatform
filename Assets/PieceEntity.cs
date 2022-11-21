using UnityEngine;

public class PieceEntity : MonoBehaviour
{

    public static float s_FallInterval = 1f;
    public static float s_RepeatInterval = 0.03f;
    public static float s_WaitBeforeRepeatInterval = 0.15f;
    public static float s_TimeTillFreeze = 1.5f;

    public Vector3 RotationPoint;

    private float _prevUpdateTime;
    private float _prevFallTime;
    private float _freezeTime;
    private float _prevBottomTouchTime;

    private bool _moveLeft = false;
    private bool _moveRight = false;
    private bool _repeat = false;
    private bool _isBottomTouched = false;

    private Playfield _playfield;

    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            FindObjectOfType<Spawner>().Swap();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _moveLeft = true;
            _moveRight = false;
            _repeat = false;
            Move(-1, 0);
            _prevUpdateTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _moveLeft = false;
            _moveRight = true;
            _repeat = false;
            Move(1, 0);
            _prevUpdateTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Rotate(false);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Rotate(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0, -1);
            _prevFallTime = Time.time;
        }
        if (!Input.GetKey(KeyCode.LeftArrow)) _moveLeft = false;
        if (!Input.GetKey(KeyCode.RightArrow)) _moveRight = false;
        if (_moveLeft || _moveRight)
        {
            int move_x = _moveLeft ? -1 : 1;
            if (_repeat)
            {
                if (Time.time - _prevUpdateTime >= s_RepeatInterval)
                {
                    Move(move_x, 0);
                    _prevUpdateTime += s_RepeatInterval;
                }
            } else
            {
                if (Time.time - _prevUpdateTime >= s_WaitBeforeRepeatInterval)
                {
                    Move(move_x, 0);
                    _prevUpdateTime += s_WaitBeforeRepeatInterval;
                    _repeat = true;
                }
            }
        } else
        {
            _repeat = false;
        }
        float interval = Input.GetKey(KeyCode.DownArrow) ? s_RepeatInterval : s_FallInterval;
        if (Time.time - _prevFallTime >= interval)
        {
            Move(0, -1);
            _prevFallTime += interval;
        }
        if (_isBottomTouched && Time.time - _prevBottomTouchTime + _freezeTime >= s_TimeTillFreeze)
        {
            _playfield.FreezePiece();
        }

    }

    public void Initialize()
    {
        _prevUpdateTime = Time.time;
        _prevFallTime = Time.time;
    }

    public void GhostDrop()
    {
        Vector3 v = new(0, -1, 0);
        while (IsValid()) transform.position += v;
        transform.position -= v;
    }

    private void HardDrop()
    {
        Vector3 v = new(0, -1, 0);
        while (IsValid()) transform.position += v;
        transform.position -= v;
        _playfield.FreezePiece();
    }
    private void Move(int x, int y)
    {
        Vector3 v = new(x, y, 0);
        transform.position += v;
        if (!IsValid()) transform.position -= v;
        PostMovement();
    }

    private void PostMovement()
    {
        BottomTest();

        // Update ghosted piece position
        PieceEntity GhostedPiece = _playfield.GhostedPiece;
        GhostedPiece.transform.position = transform.position;
        GhostedPiece.transform.rotation = transform.rotation;
        GhostedPiece.GhostDrop();
    }

    private void BottomTest()
    {
        Vector3 v = new(0, -1, 0);
        transform.position += v;
        if (!IsValid())
        {
            if (!_isBottomTouched)
            {
                _isBottomTouched = true;
                _prevBottomTouchTime = Time.time;
            }
        } else
        {
            if (_isBottomTouched)
            {
                _isBottomTouched = false;
                _freezeTime += Time.time - _prevBottomTouchTime;
                _prevBottomTouchTime = Time.time;
            }
        }
        transform.position -= v;
    }
    private void Rotate(bool isClockwise)
    {
        int degree = isClockwise ? -90 : 90;
        Vector3 v = new(0, 0, 1);
        transform.RotateAround(transform.TransformPoint(RotationPoint), v, degree);
        if (!IsValid()) transform.RotateAround(transform.TransformPoint(RotationPoint), v, -degree);
        PostMovement();
    }

    private bool IsValid()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            if (x < 0 || x >= Playfield.s_Width || y < 0 || y >= Playfield.s_Height) return false;
            if (_playfield.HasEntityAt(x, y)) return false;
        }
        return true;
    }

}

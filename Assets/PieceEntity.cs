using UnityEngine;

public class PieceEntity : MonoBehaviour
{
    public static float s_FallInterval = 1f;
    public static float s_RepeatInterval = 0.03f;
    public static float s_WaitBeforeRepeatInterval = 0.15f;
    public static float s_TimeTillFreeze = 1.5f;

    public Vector3 RotationPoint;
    private PieceShape _shape;
    public PieceShape Shape
    {
        get => _shape;
        set {
            _shape = value;
            Vector3[] localPositions = _shape.ToVector3Array();
            int i = 0;
            foreach (Transform child in transform)
            {
                child.localPosition = localPositions[i];
                i++;
            }
        }
    }

    // 0=Up, 1=Right, 2=Down, 3=Left
    public int direction;
    public PieceType PieceType;

    private float _prevUpdateTime;
    private float _prevFallTime;
    private float _freezeTime;
    private float _prevBottomTouchTime;

    private bool _moveLeft = false;
    private bool _moveRight = false;
    private bool _repeat = false;
    private bool _isBottomTouched = false;

    private Playfield _playfield;

    private int _pieceId;
    public int PieceId { 
        get => _pieceId;
        set { 
            _pieceId = value; 
            _shape = _playfield.rule.shapesOfPiece[value][direction]; 
        }
    }
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playfield.spawner.Swap();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _moveLeft = true;
            _moveRight = false;
            _repeat = false;
            Move(-1, 0);
            _prevUpdateTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _moveLeft = false;
            _moveRight = true;
            _repeat = false;
            Move(1, 0);
            _prevUpdateTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(3);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Rotate(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Rotate(2);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
        float interval = Input.GetKey(KeyCode.UpArrow) ? s_RepeatInterval : s_FallInterval;
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

    public void HardDrop()
    {
        Vector3 v = new(0, -1, 0);
        while (IsValid()) transform.position += v;
        transform.position -= v;
        _playfield.FreezePiece();
    }
    public void Move(int x, int y)
    {
        Vector3 v = new(x, y, 0);
        transform.position += v;
        if (!IsValid()) transform.position -= v;
        PostMovement();
    }

    public void PostMovement()
    {
        BottomTest();

        // Update ghosted piece position
        PieceEntity GhostedPiece = _playfield.GhostedPiece;
        GhostedPiece.transform.position = transform.position;
        GhostedPiece.GhostDrop();
    }

    public void BottomTest()
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

    public bool Rotate(int rotation)
    {
        int newDirection = (direction + rotation) % 4;
        Vector2Int[] attempts = _playfield.rule.kickTableOfPiece[PieceId][rotation, direction];

        PieceShape newShape = _playfield.rule.shapesOfPiece[PieceId][newDirection];
        int i = 0;
        foreach (Vector2Int attempt in attempts)
        {
            if(!_playfield.HitTest(transform.position, newShape, attempt))
            {
                Shape = newShape;
                _playfield.GhostedPiece.Shape = newShape;
                transform.localPosition += new Vector3(attempt.x, attempt.y, 0f);

                // Debug.Log($"rotation: {rotation} ({direction}->{newDirection}), attempt #{i} success: {attempt}");

                direction = newDirection;
                PostMovement();

                return true;
            }

            i++;
        }
        return false;

    }


    public void ResetShape()
    {
        direction = 0;
        PieceId = _pieceId;
        Shape = _shape;
    }


    private bool IsValid()
    {
        return !_playfield.HitTest(transform.position, _shape, Vector2Int.zero);
    }


}

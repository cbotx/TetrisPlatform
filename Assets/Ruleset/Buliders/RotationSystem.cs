using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RotationSystem
{
    public SpinTable[][] spinTables = new SpinTable[4][];

    public (SpinTable R, SpinTable L, SpinTable D180) this[int pieceId]
    {
        get => (
            spinTables[1][pieceId],
            spinTables[3][pieceId],
            spinTables[2][pieceId]
        );
    }

    public SpinTable[] SpinTables_R { get => spinTables[1]; set { spinTables[1] = value; } }
    public SpinTable[] SpinTables_L { get => spinTables[3]; set { spinTables[3] = value; } }
    public SpinTable[] SpinTables_180 { get => spinTables[2]; set { spinTables[2] = value; } }

    public RotationSystem(
        IEnumerable<SpinTable> spinTables__R, 
        IEnumerable<SpinTable> spinTables__L, 
        IEnumerable<SpinTable> spinTables180
    ){
        spinTables[0] = new SpinTable[] { };
        spinTables[1] = spinTables__R.ToArray();
        spinTables[2] = spinTables180.ToArray();
        spinTables[3] = spinTables__L.ToArray();
    }
    public override string ToString()
    {
        return $"R=\n{SpinTables_R}\n\nL=\n{SpinTables_L}\n\nD180=\n{SpinTables_180}";
    }

    // Z L O S I J T
    public static RotationSystem Tetris(SpinTable R, SpinTable L, SpinTable IR, SpinTable IL, SpinTable D){
        return new(
            new SpinTable[] { R, R, SpinTable.One, R, IR, R, R },
            new SpinTable[] { L, L, SpinTable.One, L, IL, L, L },
            new SpinTable[] { D, D, SpinTable.One, D,  D, D, D }
        );
    }


}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class PlusNonZero : ChangeVisitor
    {
        public override void VisitBinOpNode(BinOpNode binop)
        {
            if (binop.Left is IntNumNode && (binop.Left as IntNumNode).Num == 0 &&
            binop.Op[0] == '+')
            {
                binop.Right.Visit(this); 
                ReplaceExpr(binop, binop.Right); 
            }
            else
            {
                base.VisitBinOpNode(binop); 
            }
        }
    }
}

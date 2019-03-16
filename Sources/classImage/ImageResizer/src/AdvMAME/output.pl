$_hashFilters->{'AdvInterp3x'}     =arr(\&_voidComplex_PnQwXh    ,3,3,1,1);
$_hashFilters->{'AdvInterpH3x'}     =arr(\&_voidComplex_PnQwXh    ,3,3,1,1);
$_hashFilters->{'Eagle2x'}     =arr(\&_voidComplex_PnQwXh    ,2,2,1,1);
$_hashFilters->{'Eagle3x'}     =arr(\&_voidComplex_PnQwXh    ,3,3,1,1);
$_hashFilters->{'HQ2x'}     =arr(\&_voidComplex_nQwXh     ,2,2,1,1);
$_hashFilters->{'HQ2xBold'} =arr(\&_voidComplex_nQwXhBold ,2,2,1,1);
$_hashFilters->{'HQ2xSmart'}=arr(\&_voidComplex_nQwXhSmart,2,2,1,1);
$_hashFilters->{'HQ2x3'}     =arr(\&_voidComplex_nQwXh     ,2,3,1,1);
$_hashFilters->{'HQ2x3Bold'} =arr(\&_voidComplex_nQwXhBold ,2,3,1,1);
$_hashFilters->{'HQ2x3Smart'}=arr(\&_voidComplex_nQwXhSmart,2,3,1,1);
$_hashFilters->{'HQ2x4'}     =arr(\&_voidComplex_nQwXh     ,2,4,1,1);
$_hashFilters->{'HQ2x4Bold'} =arr(\&_voidComplex_nQwXhBold ,2,4,1,1);
$_hashFilters->{'HQ2x4Smart'}=arr(\&_voidComplex_nQwXhSmart,2,4,1,1);
$_hashFilters->{'HQ3x'}     =arr(\&_voidComplex_nQwXh     ,3,3,1,1);
$_hashFilters->{'HQ3xBold'} =arr(\&_voidComplex_nQwXhBold ,3,3,1,1);
$_hashFilters->{'HQ3xSmart'}=arr(\&_voidComplex_nQwXhSmart,3,3,1,1);
$_hashFilters->{'HQ4x'}     =arr(\&_voidComplex_nQwXh     ,4,4,1,1);
$_hashFilters->{'HQ4xBold'} =arr(\&_voidComplex_nQwXhBold ,4,4,1,1);
$_hashFilters->{'HQ4xSmart'}=arr(\&_voidComplex_nQwXhSmart,4,4,1,1);
$_hashFilters->{'LQ2x'}     =arr(\&_voidComplex_nQwXh     ,2,2,1,1);
$_hashFilters->{'LQ2xBold'} =arr(\&_voidComplex_nQwXhBold ,2,2,1,1);
$_hashFilters->{'LQ2xSmart'}=arr(\&_voidComplex_nQwXhSmart,2,2,1,1);
$_hashFilters->{'LQ2x3'}     =arr(\&_voidComplex_nQwXh     ,2,3,1,1);
$_hashFilters->{'LQ2x3Bold'} =arr(\&_voidComplex_nQwXhBold ,2,3,1,1);
$_hashFilters->{'LQ2x3Smart'}=arr(\&_voidComplex_nQwXhSmart,2,3,1,1);
$_hashFilters->{'LQ2x4'}     =arr(\&_voidComplex_nQwXh     ,2,4,1,1);
$_hashFilters->{'LQ2x4Bold'} =arr(\&_voidComplex_nQwXhBold ,2,4,1,1);
$_hashFilters->{'LQ2x4Smart'}=arr(\&_voidComplex_nQwXhSmart,2,4,1,1);
$_hashFilters->{'LQ3x'}     =arr(\&_voidComplex_nQwXh     ,3,3,1,1);
$_hashFilters->{'LQ3xBold'} =arr(\&_voidComplex_nQwXhBold ,3,3,1,1);
$_hashFilters->{'LQ3xSmart'}=arr(\&_voidComplex_nQwXhSmart,3,3,1,1);
$_hashFilters->{'LQ4x'}     =arr(\&_voidComplex_nQwXh     ,4,4,1,1);
$_hashFilters->{'LQ4xBold'} =arr(\&_voidComplex_nQwXhBold ,4,4,1,1);
$_hashFilters->{'LQ4xSmart'}=arr(\&_voidComplex_nQwXhSmart,4,4,1,1);
$_hashFilters->{'Scale2x'}     =arr(\&_voidComplex_PnQwXh    ,2,2,1,1);

# standard AdvInterp3x casepath
sub _arrAdvInterp3x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8
  )=@_;
  my ( $varE00, $varE01, $varE02,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22,  ) = ( $varC4, $varC4, $varC4, );
  #BEGIN AdvInterp3x PATTERNS

    if (_boolNE( $varC1, $varC7 ) and _boolNE( $varC3, $varC5 )) {
  if (_boolE( $varC1, $varC3 )) {
        $varE00=_varInterp2($varC3,$varC4,5,3);
  };

  if (
    (_boolE( $varC1, $varC3 ) and _boolNE( $varC4, $varC2 )) or
    (_boolE( $varC1, $varC5 ) and _boolNE( $varC4, $varC0 ))
  ) {
         $varE01=$varC1; 
  };

  if (_boolE( $varC1, $varC5 )) {
        $varE02=_varInterp2($varC5,$varC4,5,3);
  };

  if (
    (_boolE( $varC1, $varC3 ) and _boolNE( $varC4, $varC6 )) or
    (_boolE( $varC7, $varC3 ) and _boolNE( $varC4, $varC0 ))
  ) {
        $varE10=$varC3;
  };
      $varE11=$varC4;
  if (
    (_boolE( $varC1, $varC5 ) and _boolNE( $varC4, $varC8 )) or
    (_boolE( $varC7, $varC5 ) and _boolNE( $varC4, $varC2 ))
  ) {
        $varE12=$varC5;
  };

  if (_boolE( $varC7, $varC3 )) {
        $varE20=_varInterp2($varC3,$varC4,5,3);
  };

  if (
    (_boolE( $varC7, $varC3 ) and _boolNE( $varC4, $varC8 )) or
    (_boolE( $varC7, $varC5 ) and _boolNE( $varC4, $varC6 ))
  ) { 
        $varE21=$varC7;
  };

  if (_boolE( $varC7, $varC5 )) {
        $varE22=_varInterp2($varC5,$varC4,5,3);
  };
    }

  #END AdvInterp3x PATTERNS
  return (
    $varE00, $varE01, $varE02, 
    $varE10, $varE11, $varE12, 
    $varE20, $varE21, $varE22, 
  );
}

# standard AdvInterpH3x casepath
sub _arrAdvInterpH3x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8
  )=@_;
  my ( $varE00, $varE01, $varE02,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22,  ) = ( $varC4, $varC4, $varC4, );
  #BEGIN AdvInterpH3x PATTERNS
if (_boolYUV_NE( $varC4, $varC0 )) {
  if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00=_varInterp2($varC1,$varC3,1,1);
      }
    } else {
      $varE01=_varInterp2($varC0,$varC4,1,1);
      $varE10=_varInterp2($varC0,$varC4,1,1);
    }

    if (_boolYUV_NE( $varC4, $varC2 )) {
  if (_boolYUV_E( $varC1, $varC5 )) {
        $varE02=_varInterp2($varC1,$varC5,1,1);
      }
    } else {
      $varE01=_varInterp2($varC2,$varC4,1,1);
      $varE12=_varInterp2($varC2,$varC4,1,1);
    }

    if (_boolYUV_NE( $varC4, $varC6 )) {
  if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20=_varInterp2($varC7,$varC3,1,1);
      }
    } else {
      $varE10=_varInterp2($varC6,$varC4,1,1);
      $varE21=_varInterp2($varC6,$varC4,1,1);
    }

    if (_boolYUV_NE( $varC4, $varC8 )) {
  if (_boolYUV_E( $varC7, $varC5 )) {
        $varE22=_varInterp2($varC7,$varC5,1,1);
      }
    } else {
      $varE21=_varInterp2($varC8,$varC4,1,1);
      $varE12=_varInterp2($varC8,$varC4,1,1);
}
  #END AdvInterpH3x PATTERNS
  return (
    $varE00, $varE01, $varE02, 
    $varE10, $varE11, $varE12, 
    $varE20, $varE21, $varE22, 
  );
}

# standard Eagle2x casepath
sub _arrEagle2x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  #BEGIN Eagle2x PATTERNS

    if (_boolYUV_E( $varC1, $varC0 ) and _boolYUV_E( $varC1, $varC3 )) {
      $varE00=_varInterp3($varC0,$varC1,$varC3,1,1,1);
    }

    if (_boolYUV_E( $varC1, $varC2 ) and _boolYUV_E( $varC1, $varC5 )) {
      $varE01=_varInterp3($varC1,$varC2,$varC5,1,1,1);
    }

    if (_boolYUV_E( $varC7, $varC6 ) and _boolYUV_E( $varC7, $varC3 )) {
      $varE10=_varInterp3($varC3,$varC6,$varC7,1,1,1);
    }

    if (_boolYUV_E( $varC7, $varC8 ) and _boolYUV_E( $varC7, $varC5 )) {
      $varE11=_varInterp3($varC5,$varC7,$varC8,1,1,1);
}
  #END Eagle2x PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
  );
}

# standard Eagle3x casepath
sub _arrEagle3x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8
  )=@_;
  my ( $varE00, $varE01, $varE02,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22,  ) = ( $varC4, $varC4, $varC4, );
  #BEGIN Eagle3x PATTERNS

    if (_boolYUV_E( $varC1, $varC0 ) and _boolYUV_E( $varC1, $varC3 )) {
      $varE00=_varInterp3($varC0,$varC1,$varC3,1,1,1);
    }

    if (_boolYUV_E( $varC1, $varC2 ) and _boolYUV_E( $varC1, $varC5 )) {
      $varE02=_varInterp3($varC1,$varC2,$varC5,1,1,1);
    }

    if (_boolYUV_E( $varC7, $varC6 ) and _boolYUV_E( $varC7, $varC3 )) {
      $varE20=_varInterp3($varC3,$varC6,$varC7,1,1,1);
    }

    if (_boolYUV_E( $varC7, $varC8 ) and _boolYUV_E( $varC7, $varC5 )) {
      $varE22=_varInterp3($varC5,$varC7,$varC8,1,1,1);
    }

    if (_boolYUV_E( $varC1, $varC0 ) and _boolYUV_E( $varC1, $varC3 ) and _boolYUV_E( $varC1, $varC2 ) and _boolYUV_E( $varC1, $varC5 )) {
  my $varA=_varInterp3($varC0,$varC1,$varC3,1,1,1);
  my $varB=_varInterp3($varC1,$varC2,$varC5,1,1,1);
      $varE01=_varInterp2($varA,$varB,1,1);
    }

    if (_boolYUV_E( $varC1, $varC2 ) and _boolYUV_E( $varC1, $varC5 ) and _boolYUV_E( $varC7, $varC8 ) and _boolYUV_E( $varC7, $varC5 )) {
  my $varA=_varInterp3($varC1,$varC2,$varC5,1,1,1);
  my $varB=_varInterp3($varC5,$varC7,$varC8,1,1,1);
      $varE12=_varInterp2($varA,$varB,1,1);
    }

    if (_boolYUV_E( $varC7, $varC6 ) and _boolYUV_E( $varC7, $varC3 ) and _boolYUV_E( $varC7, $varC8 ) and _boolYUV_E( $varC7, $varC5 )) {
  my $varA=_varInterp3($varC3,$varC6,$varC7,1,1,1);
  my $varB=_varInterp3($varC5,$varC7,$varC8,1,1,1);
      $varE21=_varInterp2($varA,$varB,1,1);
    }

    if (_boolYUV_E( $varC1, $varC0 ) and _boolYUV_E( $varC1, $varC3 ) and _boolYUV_E( $varC7, $varC6 ) and _boolYUV_E( $varC7, $varC3 )) {
  my $varA=_varInterp3($varC0,$varC1,$varC3,1,1,1);
  my $varB=_varInterp3($varC3,$varC6,$varC7,1,1,1);
      $varE10=_varInterp2($varA,$varB,1,1);
}
  #END Eagle3x PATTERNS
  return (
    $varE00, $varE01, $varE02, 
    $varE10, $varE11, $varE12, 
    $varE20, $varE21, $varE22, 
  );
}

# standard HQ2x casepath
sub _arrHQ2x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  #BEGIN HQ2x PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10100010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10100110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b10001011 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
        $varE01 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00110010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00110110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b00011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00011010 ) or 
    ( $intPattern == 0b00011111 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE10 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE10 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00111010 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b00111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp3($varC4,$varC7,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01000010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01000110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
        $varE10 = _varInterp3($varC3,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) or 
    ( $intPattern == 0b01101011 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001011 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b01010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b01010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01010110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01100010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01100110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
  } elsif (
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
        $varE10 = _varInterp3($varC3,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01101010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC8,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b01110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
        $varE10 = _varInterp3($varC3,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10110010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
        $varE11 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC4,3,3,2);
        $varE11 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC4,3,3,2);
        $varE11 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011010 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE10 = _varInterp3($varC4,$varC6,$varC7,2,1,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111010 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE10 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b10111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC4,3,3,2);
        $varE11 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC7,3,1);
    $varE11 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE10 = _varInterp3($varC3,$varC7,$varC4,3,3,2);
        $varE11 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001010 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001011 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010100 ) or 
    ( $intPattern == 0b11010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC6,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE10 = _varInterp3($varC3,$varC7,$varC4,3,3,2);
        $varE11 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC2,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE10 = _varInterp3($varC3,$varC7,$varC4,3,3,2);
        $varE11 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110000 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC4,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC3,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC2,2,1,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  }
  #END HQ2x PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
  );
}

# standard HQ2x3 casepath
sub _arrHQ2x3 {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  my ( $varE20, $varE21,  ) = ( $varC4, $varC4, );
  #BEGIN HQ2x3 PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b10001011 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = _varInterp2($varC4,$varC5,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,10,5,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,7,6,3);
        $varE10 = _varInterp2($varC4,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,10,5,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,7,6,3);
        $varE10 = _varInterp2($varC4,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00110010 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,13,3);
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,7,6,3);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,10,5,1);
        $varE11 = _varInterp2($varC4,$varC5,13,3);
    }
  } elsif (
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00110110 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,7,6,3);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,10,5,1);
        $varE11 = _varInterp2($varC4,$varC5,13,3);
    }
  } elsif (
    ( $intPattern == 0b00011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00011010 ) or 
    ( $intPattern == 0b00011111 ) 
  ){
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00011011 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00011110 ) 
  ){
    $varE10 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,5,4);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,5,4);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b00111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00111010 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_E( $varC1, $varC5 )) {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_E( $varC1, $varC3 ))) {
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
  } elsif (
    ( $intPattern == 0b00111110 ) 
  ){
    $varE10 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE10 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE20 = _varInterp3($varC7,$varC3,$varC4,7,5,4);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) or 
    ( $intPattern == 0b01101011 ) 
  ){
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001011 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC5,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b01010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b01010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
        $varE11 = _varInterp2($varC4,$varC5,1,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC7,$varC5,$varC4,7,5,4);
    }
  } elsif (
    ( $intPattern == 0b01010110 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC3,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    $varE11 = $varC4;
    if (_boolYUV_E( $varC1, $varC5 )) {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_E( $varC1, $varC3 ))) {
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    $varE10 = $varC4;
    if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if ((_boolYUV_E( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01011111 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
  } elsif (
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE20 = _varInterp3($varC7,$varC3,$varC4,7,5,4);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b01110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,13,3);
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE11 = _varInterp2($varC4,$varC5,13,3);
        $varE20 = _varInterp3($varC7,$varC4,$varC3,7,6,3);
        $varE21 = _varInterp3($varC7,$varC5,$varC4,10,5,1);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110110 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,7,6,3);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,10,5,1);
        $varE11 = _varInterp2($varC4,$varC5,13,3);
    }
  } elsif (
    ( $intPattern == 0b01111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE11 = $varC4;
    if (_boolYUV_E( $varC7, $varC5 )) {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_E( $varC7, $varC3 ))) {
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    $varE11 = $varC4;
    if (_boolYUV_E( $varC7, $varC5 )) {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_E( $varC7, $varC3 ))) {
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01111011 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE20 = _varInterp3($varC7,$varC3,$varC4,7,5,4);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    if (_boolYUV_E( $varC1, $varC5 )) {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_E( $varC1, $varC3 ))) {
        $varE01 = _varInterp2($varC4,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE01 = $varC4;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC8,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC8,$varC7,12,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10110010 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE11 = $varC4;
        $varE21 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,7,5,4);
        $varE11 = _varInterp2($varC4,$varC5,1,1);
        $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110110 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
        $varE21 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,7,5,4);
        $varE11 = _varInterp2($varC4,$varC5,1,1);
        $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011010 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10011011 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b10011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE10 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if ((_boolYUV_E( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111010 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10111011 ) 
  ){
    $varE11 = $varC4;
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,5,4);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE20 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    }
  } elsif (
    ( $intPattern == 0b10111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111110 ) 
  ){
    $varE10 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
        $varE11 = $varC4;
        $varE21 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC4,7,5,4);
        $varE11 = _varInterp2($varC4,$varC5,1,1);
        $varE21 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = _varInterp2($varC4,$varC5,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,13,3);
        $varE20 = _varInterp3($varC7,$varC3,$varC4,10,5,1);
        $varE21 = _varInterp3($varC7,$varC4,$varC5,7,6,3);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11001010 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11001011 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11001111 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,10,5,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,7,6,3);
        $varE10 = _varInterp2($varC4,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11010010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11010100 ) or 
    ( $intPattern == 0b11010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
        $varE11 = _varInterp2($varC4,$varC5,1,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC7,$varC5,$varC4,7,5,4);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    $varE10 = $varC4;
    if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if ((_boolYUV_E( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if ((_boolYUV_E( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
        $varE11 = _varInterp2($varC4,$varC5,1,1);
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC7,$varC5,$varC4,7,5,4);
    }
  } elsif (
    ( $intPattern == 0b11011110 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
    }
    if ((_boolYUV_E( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp2($varC4,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = $varC4;
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC6,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
  } elsif (
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC5,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,13,3);
        $varE20 = _varInterp3($varC7,$varC3,$varC4,10,5,1);
        $varE21 = _varInterp3($varC7,$varC4,$varC5,7,6,3);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC5,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,12,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = _varInterp2($varC4,$varC5,13,3);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,13,3);
        $varE20 = _varInterp3($varC7,$varC3,$varC4,10,5,1);
        $varE21 = _varInterp3($varC7,$varC4,$varC5,7,6,3);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,13,3);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,13,3);
    $varE21 = _varInterp2($varC4,$varC5,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11110000 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,13,3);
        $varE20 = _varInterp3($varC7,$varC4,$varC3,7,6,3);
        $varE21 = _varInterp3($varC7,$varC5,$varC4,10,5,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,12,3,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,13,3);
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,13,3);
        $varE20 = _varInterp3($varC7,$varC4,$varC3,7,6,3);
        $varE21 = _varInterp3($varC7,$varC5,$varC4,10,5,1);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,13,3);
    $varE10 = _varInterp2($varC4,$varC3,13,3);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,13,3);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    $varE10 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,13,3);
    $varE01 = _varInterp2($varC4,$varC2,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    if (_boolYUV_E( $varC7, $varC5 )) {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_E( $varC7, $varC3 ))) {
        $varE21 = _varInterp2($varC4,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE21 = $varC4;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp3($varC4,$varC2,$varC1,12,3,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
    }
    if ((_boolYUV_E( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp2($varC4,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = $varC4;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC0,13,3);
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC0,$varC1,12,3,1);
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  }
  #END HQ2x3 PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
    $varE20, $varE21, 
  );
}

# standard HQ2x4 casepath
sub _arrHQ2x4 {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  my ( $varE20, $varE21,  ) = ( $varC4, $varC4, );
  my ( $varE30, $varE31,  ) = ( $varC4, $varC4, );
  #BEGIN HQ2x4 PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b10001011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE01 = _varInterp2($varC4,$varC5,3,1);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,9,7);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,9,7);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE00 = _varInterp2($varC1,$varC4,1,1);
        $varE01 = _varInterp2($varC1,$varC5,9,7);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC4,1,1);
        $varE01 = _varInterp2($varC1,$varC5,9,7);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00011010 ) or 
    ( $intPattern == 0b00011111 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00011011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
        $varE20 = _varInterp2($varC4,$varC7,7,1);
        $varE30 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,4,3,1);
        $varE10 = _varInterp3($varC3,$varC4,$varC1,3,3,2);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,9,6,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,11,3,2);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,7,1);
        $varE30 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,4,3,1);
        $varE10 = _varInterp3($varC3,$varC4,$varC1,3,3,2);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,9,6,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,11,3,2);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC7,11,3,2);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC5,9,4,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00111010 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
  } elsif (
    ( $intPattern == 0b00111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE10 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp3($varC4,$varC8,$varC7,5,2,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC1,7,1);
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,11,3,2);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,9,6,1);
        $varE20 = _varInterp3($varC3,$varC4,$varC7,3,3,2);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) or 
    ( $intPattern == 0b01101011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b01010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b01010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,5,3);
        $varE11 = _varInterp2($varC4,$varC1,7,1);
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,11,3,2);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,9,6,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,3,3,2);
        $varE31 = _varInterp3($varC7,$varC5,$varC4,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011111 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
  } elsif (
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) 
  ){
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC1,7,1);
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,11,3,2);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,9,6,1);
        $varE20 = _varInterp3($varC3,$varC4,$varC7,3,3,2);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp3($varC4,$varC5,$varC8,6,1,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b01110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE30 = _varInterp2($varC4,$varC3,3,1);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,8,5,3);
        $varE30 = _varInterp2($varC4,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC5,9,7);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC4,1,1);
        $varE01 = _varInterp2($varC1,$varC5,9,7);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b01111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = _varInterp2($varC4,$varC8,13,3);
        $varE31 = _varInterp2($varC4,$varC8,11,5);
    } else {
        $varE21 = _varInterp2($varC4,$varC5,7,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC1,7,1);
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,11,3,2);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,9,6,1);
        $varE20 = _varInterp3($varC3,$varC4,$varC7,3,3,2);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE10 = $varC4;
    $varE21 = _varInterp2($varC4,$varC8,13,3);
    $varE31 = _varInterp2($varC4,$varC8,11,5);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE31 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC4,4,3,1);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,3,3,2);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,9,6,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,11,3,2);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE31 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC4,4,3,1);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,3,3,2);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,9,6,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,11,3,2);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,11,3,2);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC7,$varC3,9,4,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011010 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10011011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b10011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC6,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111010 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10111011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,7,1);
        $varE30 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,4,3,1);
        $varE10 = _varInterp3($varC3,$varC4,$varC1,3,3,2);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,9,6,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,11,3,2);
    }
  } elsif (
    ( $intPattern == 0b10111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE31 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC4,4,3,1);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,3,3,2);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,9,6,1);
        $varE31 = _varInterp3($varC4,$varC7,$varC5,11,3,2);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
        $varE31 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,8,5,3);
        $varE30 = _varInterp2($varC7,$varC3,9,7);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b11001010 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11001011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11001111 ) 
  ){
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = _varInterp2($varC4,$varC5,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,9,7);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010100 ) or 
    ( $intPattern == 0b11010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,5,3);
        $varE11 = _varInterp2($varC4,$varC1,7,1);
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,11,3,2);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,9,6,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,3,3,2);
        $varE31 = _varInterp3($varC7,$varC5,$varC4,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC6,6,1,1);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,13,3);
        $varE30 = _varInterp2($varC4,$varC6,11,5);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC1,5,3);
        $varE11 = _varInterp2($varC4,$varC1,7,1);
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,11,3,2);
        $varE11 = _varInterp3($varC4,$varC5,$varC1,9,6,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,3,3,2);
        $varE31 = _varInterp3($varC7,$varC5,$varC4,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,13,3);
    $varE30 = _varInterp2($varC4,$varC6,11,5);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,8,5,3);
        $varE30 = _varInterp2($varC7,$varC3,9,7);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC5,9,4,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp3($varC4,$varC5,$varC1,11,3,2);
    $varE20 = $varC4;
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,11,5);
        $varE10 = _varInterp2($varC4,$varC0,13,3);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp3($varC4,$varC2,$varC5,6,1,1);
    $varE20 = $varC4;
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,8,5,3);
        $varE30 = _varInterp2($varC7,$varC3,9,7);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = $varC4;
    $varE21 = _varInterp2($varC4,$varC5,3,1);
    $varE31 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110000 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE30 = _varInterp2($varC4,$varC3,3,1);
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,8,5,3);
        $varE30 = _varInterp2($varC4,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC5,9,7);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = _varInterp2($varC4,$varC2,11,5);
        $varE11 = _varInterp2($varC4,$varC2,13,3);
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE30 = _varInterp2($varC4,$varC3,3,1);
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,8,5,3);
        $varE30 = _varInterp2($varC4,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC5,9,7);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,9,4,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,11,3,2);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp3($varC4,$varC0,$varC3,6,1,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp3($varC4,$varC2,$varC1,5,2,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC2,11,5);
    $varE11 = _varInterp2($varC4,$varC2,13,3);
    $varE20 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC0,$varC1,5,2,1);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,11,5);
    $varE10 = _varInterp2($varC4,$varC0,13,3);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  }
  #END HQ2x4 PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
    $varE20, $varE21, 
    $varE30, $varE31, 
  );
}

# standard HQ3x casepath
sub _arrHQ3x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01, $varE02,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22,  ) = ( $varC4, $varC4, $varC4, );
  #BEGIN HQ3x PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b10001011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC5,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) 
  ){
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC5,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE12 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00011000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00011010 ) or 
    ( $intPattern == 0b00011111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00011011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
        $varE01 = $varC4;
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00111010 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b00111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) or 
    ( $intPattern == 0b01101011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b01010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b01010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = _varInterp2($varC5,$varC4,3,1);
        $varE21 = _varInterp2($varC4,$varC7,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
  } elsif (
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b01110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE12 = _varInterp2($varC4,$varC5,3,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE21 = _varInterp2($varC7,$varC4,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,3,1);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE11 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE12 = $varC4;
        $varE22 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC5,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
        $varE22 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC5,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011010 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b10011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111010 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
  } elsif (
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
        $varE22 = _varInterp2($varC4,$varC7,3,1);
    } else {
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC5,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,3,1);
    $varE21 = _varInterp2($varC4,$varC7,3,1);
    $varE22 = _varInterp2($varC4,$varC7,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = $varC4;
        $varE22 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC7,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001010 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001111 ) 
  ){
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC5,3,1);
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11010010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11010100 ) or 
    ( $intPattern == 0b11010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = _varInterp2($varC5,$varC4,3,1);
        $varE21 = _varInterp2($varC4,$varC7,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = _varInterp2($varC5,$varC4,3,1);
        $varE21 = _varInterp2($varC4,$varC7,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
  } elsif (
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
        $varE22 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC7,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,3,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
        $varE22 = _varInterp2($varC4,$varC5,3,1);
    } else {
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC7,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110000 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,3,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE21 = _varInterp2($varC7,$varC4,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,3,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE21 = _varInterp2($varC7,$varC4,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,3,1);
    $varE01 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,3,1);
    $varE01 = _varInterp2($varC4,$varC1,3,1);
    $varE02 = _varInterp2($varC4,$varC1,3,1);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
        $varE20 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC4;
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE12 = _varInterp2($varC4,$varC5,7,1);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  }
  #END HQ3x PATTERNS
  return (
    $varE00, $varE01, $varE02, 
    $varE10, $varE11, $varE12, 
    $varE20, $varE21, $varE22, 
  );
}

# standard HQ4x casepath
sub _arrHQ4x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01, $varE02, $varE03,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12, $varE13,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22, $varE23,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  my ( $varE30, $varE31, $varE32, $varE33,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  #BEGIN HQ4x PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
        $varE11 = $varC4;
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b10001011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE02 = _varInterp2($varC4,$varC5,7,1);
        $varE03 = _varInterp2($varC4,$varC5,5,3);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC3,5,3);
        $varE02 = _varInterp2($varC1,$varC4,3,1);
        $varE03 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp3($varC3,$varC1,$varC4,2,1,1);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) 
  ){
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC5,7,1);
        $varE03 = _varInterp2($varC4,$varC5,5,3);
        $varE10 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC3,5,3);
        $varE02 = _varInterp2($varC1,$varC4,3,1);
        $varE03 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp3($varC3,$varC1,$varC4,2,1,1);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,5,3);
        $varE01 = _varInterp2($varC4,$varC3,7,1);
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC1,$varC5,5,3);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp3($varC5,$varC1,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,5,3);
        $varE01 = _varInterp2($varC4,$varC3,7,1);
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE12 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC1,$varC5,5,3);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp3($varC5,$varC1,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00011010 ) or 
    ( $intPattern == 0b00011111 ) 
  ){
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
        $varE20 = _varInterp2($varC4,$varC7,7,1);
        $varE30 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC3,$varC1,5,3);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
        $varE20 = _varInterp2($varC3,$varC4,3,1);
        $varE30 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,7,1);
        $varE30 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC3,$varC1,5,3);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
        $varE20 = _varInterp2($varC3,$varC4,3,1);
        $varE30 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC7,5,2,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC5,5,2,1);
    $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00111010 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE11 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b00111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp3($varC4,$varC7,$varC8,5,2,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE21 = $varC4;
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC1,7,1);
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp2($varC3,$varC7,5,3);
        $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) or 
    ( $intPattern == 0b01101011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b01010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b01010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE03 = _varInterp2($varC4,$varC1,5,3);
        $varE13 = _varInterp2($varC4,$varC1,7,1);
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE03 = _varInterp2($varC4,$varC5,3,1);
        $varE13 = _varInterp2($varC5,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp2($varC5,$varC7,5,3);
        $varE32 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    $varE12 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011111 ) 
  ){
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
  } elsif (
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC1,7,1);
        $varE20 = $varC4;
        $varE21 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp2($varC3,$varC7,5,3);
        $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp3($varC4,$varC5,$varC8,5,2,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b01110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE30 = _varInterp2($varC4,$varC3,5,3);
        $varE31 = _varInterp2($varC4,$varC3,7,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp3($varC5,$varC4,$varC7,2,1,1);
        $varE30 = _varInterp2($varC4,$varC7,3,1);
        $varE31 = _varInterp2($varC7,$varC4,3,1);
        $varE32 = _varInterp2($varC7,$varC5,5,3);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = _varInterp2($varC4,$varC3,5,3);
        $varE01 = _varInterp2($varC4,$varC3,7,1);
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE12 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE00 = _varInterp2($varC4,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC1,$varC5,5,3);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp3($varC5,$varC1,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = _varInterp2($varC4,$varC8,7,1);
        $varE23 = _varInterp2($varC4,$varC8,3,1);
        $varE32 = _varInterp2($varC4,$varC8,3,1);
        $varE33 = _varInterp2($varC4,$varC8,5,3);
    } else {
        $varE22 = $varC4;
        $varE23 = _varInterp2($varC4,$varC5,3,1);
        $varE32 = _varInterp2($varC4,$varC7,3,1);
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC1,7,1);
        $varE20 = $varC4;
        $varE21 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE00 = _varInterp2($varC4,$varC3,3,1);
        $varE10 = _varInterp2($varC3,$varC4,3,1);
        $varE20 = _varInterp2($varC3,$varC7,5,3);
        $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC8,7,1);
    $varE23 = _varInterp2($varC4,$varC8,3,1);
    $varE32 = _varInterp2($varC4,$varC8,3,1);
    $varE33 = _varInterp2($varC4,$varC8,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
        $varE23 = _varInterp2($varC4,$varC7,7,1);
        $varE33 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE02 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp2($varC5,$varC1,5,3);
        $varE23 = _varInterp2($varC5,$varC4,3,1);
        $varE33 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE12 = $varC4;
        $varE13 = $varC4;
        $varE23 = _varInterp2($varC4,$varC7,7,1);
        $varE33 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE02 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp2($varC5,$varC1,5,3);
        $varE23 = _varInterp2($varC5,$varC4,3,1);
        $varE33 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC7,5,2,1);
    $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    $varE31 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011010 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10011011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE02 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp3($varC4,$varC7,$varC6,5,2,1);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111010 ) 
  ){
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10111011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
        $varE11 = $varC4;
        $varE20 = _varInterp2($varC4,$varC7,7,1);
        $varE30 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC3,$varC1,5,3);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
        $varE20 = _varInterp2($varC3,$varC4,3,1);
        $varE30 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b10111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
  } elsif (
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE12 = $varC4;
        $varE13 = $varC4;
        $varE23 = _varInterp2($varC4,$varC7,7,1);
        $varE33 = _varInterp2($varC4,$varC7,5,3);
    } else {
        $varE02 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp2($varC5,$varC1,5,3);
        $varE23 = _varInterp2($varC5,$varC4,3,1);
        $varE33 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = _varInterp2($varC4,$varC7,7,1);
    $varE21 = _varInterp2($varC4,$varC7,7,1);
    $varE22 = _varInterp2($varC4,$varC7,7,1);
    $varE23 = _varInterp2($varC4,$varC7,7,1);
    $varE30 = _varInterp2($varC4,$varC7,5,3);
    $varE31 = _varInterp2($varC4,$varC7,5,3);
    $varE32 = _varInterp2($varC4,$varC7,5,3);
    $varE33 = _varInterp2($varC4,$varC7,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11000010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11000011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11000110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11000111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
        $varE32 = _varInterp2($varC4,$varC5,7,1);
        $varE33 = _varInterp2($varC4,$varC5,5,3);
    } else {
        $varE20 = _varInterp3($varC3,$varC4,$varC7,2,1,1);
        $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC3,5,3);
        $varE32 = _varInterp2($varC7,$varC4,3,1);
        $varE33 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11001010 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
  } elsif (
    ( $intPattern == 0b11001011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001110 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
  } elsif (
    ( $intPattern == 0b11001111 ) 
  ){
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE02 = _varInterp2($varC4,$varC5,7,1);
        $varE03 = _varInterp2($varC4,$varC5,5,3);
        $varE10 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC3,5,3);
        $varE02 = _varInterp2($varC1,$varC4,3,1);
        $varE03 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp3($varC3,$varC1,$varC4,2,1,1);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010100 ) or 
    ( $intPattern == 0b11010101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE03 = _varInterp2($varC4,$varC1,5,3);
        $varE13 = _varInterp2($varC4,$varC1,7,1);
        $varE22 = $varC4;
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE03 = _varInterp2($varC4,$varC5,3,1);
        $varE13 = _varInterp2($varC5,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp2($varC5,$varC7,5,3);
        $varE32 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = _varInterp3($varC4,$varC3,$varC6,5,2,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = _varInterp2($varC4,$varC6,3,1);
        $varE21 = _varInterp2($varC4,$varC6,7,1);
        $varE30 = _varInterp2($varC4,$varC6,5,3);
        $varE31 = _varInterp2($varC4,$varC6,3,1);
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE21 = $varC4;
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE03 = _varInterp2($varC4,$varC1,5,3);
        $varE13 = _varInterp2($varC4,$varC1,7,1);
        $varE22 = $varC4;
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE03 = _varInterp2($varC4,$varC5,3,1);
        $varE13 = _varInterp2($varC5,$varC4,3,1);
        $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp2($varC5,$varC7,5,3);
        $varE32 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE02 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = _varInterp2($varC4,$varC6,3,1);
    $varE21 = _varInterp2($varC4,$varC6,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC6,5,3);
    $varE31 = _varInterp2($varC4,$varC6,3,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11100010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
  } elsif (
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
        $varE32 = _varInterp2($varC4,$varC5,7,1);
        $varE33 = _varInterp2($varC4,$varC5,5,3);
    } else {
        $varE20 = _varInterp3($varC3,$varC4,$varC7,2,1,1);
        $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC3,5,3);
        $varE32 = _varInterp2($varC7,$varC4,3,1);
        $varE33 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC5,5,2,1);
    $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC1,5,2,1);
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE31 = $varC4;
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = _varInterp2($varC4,$varC0,5,3);
        $varE01 = _varInterp2($varC4,$varC0,3,1);
        $varE10 = _varInterp2($varC4,$varC0,3,1);
        $varE11 = _varInterp2($varC4,$varC0,7,1);
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
        $varE11 = $varC4;
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp3($varC4,$varC5,$varC2,5,2,1);
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE31 = $varC4;
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
        $varE32 = _varInterp2($varC4,$varC5,7,1);
        $varE33 = _varInterp2($varC4,$varC5,5,3);
    } else {
        $varE20 = _varInterp3($varC3,$varC4,$varC7,2,1,1);
        $varE21 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC3,5,3);
        $varE32 = _varInterp2($varC7,$varC4,3,1);
        $varE33 = _varInterp2($varC4,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = _varInterp2($varC4,$varC5,7,1);
    $varE03 = _varInterp2($varC4,$varC5,5,3);
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC5,7,1);
    $varE13 = _varInterp2($varC4,$varC5,5,3);
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = _varInterp2($varC4,$varC5,7,1);
    $varE23 = _varInterp2($varC4,$varC5,5,3);
    $varE31 = $varC4;
    $varE32 = _varInterp2($varC4,$varC5,7,1);
    $varE33 = _varInterp2($varC4,$varC5,5,3);
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110000 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
        $varE23 = $varC4;
        $varE30 = _varInterp2($varC4,$varC3,5,3);
        $varE31 = _varInterp2($varC4,$varC3,7,1);
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp3($varC5,$varC4,$varC7,2,1,1);
        $varE30 = _varInterp2($varC4,$varC7,3,1);
        $varE31 = _varInterp2($varC7,$varC4,3,1);
        $varE32 = _varInterp2($varC7,$varC5,5,3);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = _varInterp2($varC4,$varC2,3,1);
        $varE03 = _varInterp2($varC4,$varC2,5,3);
        $varE12 = _varInterp2($varC4,$varC2,7,1);
        $varE13 = _varInterp2($varC4,$varC2,3,1);
    } else {
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = $varC4;
        $varE13 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
        $varE23 = $varC4;
        $varE30 = _varInterp2($varC4,$varC3,5,3);
        $varE31 = _varInterp2($varC4,$varC3,7,1);
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp3($varC5,$varC4,$varC7,2,1,1);
        $varE30 = _varInterp2($varC4,$varC7,3,1);
        $varE31 = _varInterp2($varC7,$varC4,3,1);
        $varE32 = _varInterp2($varC7,$varC5,5,3);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    $varE01 = _varInterp3($varC4,$varC1,$varC3,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp3($varC4,$varC3,$varC1,5,2,1);
    $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp3($varC4,$varC3,$varC0,5,2,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC3,5,3);
    $varE01 = _varInterp2($varC4,$varC3,7,1);
    $varE02 = $varC4;
    $varE10 = _varInterp2($varC4,$varC3,5,3);
    $varE11 = _varInterp2($varC4,$varC3,7,1);
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = _varInterp2($varC4,$varC3,5,3);
    $varE21 = _varInterp2($varC4,$varC3,7,1);
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = _varInterp2($varC4,$varC3,5,3);
    $varE31 = _varInterp2($varC4,$varC3,7,1);
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp3($varC4,$varC1,$varC2,5,2,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE02 = _varInterp2($varC4,$varC2,3,1);
    $varE03 = _varInterp2($varC4,$varC2,5,3);
    $varE11 = $varC4;
    $varE12 = _varInterp2($varC4,$varC2,7,1);
    $varE13 = _varInterp2($varC4,$varC2,3,1);
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp3($varC4,$varC1,$varC0,5,2,1);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC1,5,3);
    $varE01 = _varInterp2($varC4,$varC1,5,3);
    $varE02 = _varInterp2($varC4,$varC1,5,3);
    $varE03 = _varInterp2($varC4,$varC1,5,3);
    $varE10 = _varInterp2($varC4,$varC1,7,1);
    $varE11 = _varInterp2($varC4,$varC1,7,1);
    $varE12 = _varInterp2($varC4,$varC1,7,1);
    $varE13 = _varInterp2($varC4,$varC1,7,1);
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = _varInterp2($varC4,$varC0,5,3);
    $varE01 = _varInterp2($varC4,$varC0,3,1);
    $varE10 = _varInterp2($varC4,$varC0,3,1);
    $varE11 = _varInterp2($varC4,$varC0,7,1);
    $varE12 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  }
  #END HQ4x PATTERNS
  return (
    $varE00, $varE01, $varE02, $varE03, 
    $varE10, $varE11, $varE12, $varE13, 
    $varE20, $varE21, $varE22, $varE23, 
    $varE30, $varE31, $varE32, $varE33, 
  );
}

# standard LQ2x casepath
sub _arrLQ2x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  #BEGIN LQ2x PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00011000 ) or 
    ( $intPattern == 0b00011100 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00111000 ) or 
    ( $intPattern == 0b00111100 ) or 
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000010 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000110 ) or 
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100010 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100110 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) or 
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10011000 ) or 
    ( $intPattern == 0b10011100 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100010 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100110 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10111000 ) or 
    ( $intPattern == 0b10111100 ) or 
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000010 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000110 ) or 
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100010 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
  } elsif (
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00011001 ) or 
    ( $intPattern == 0b00011101 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b00110001 ) or 
    ( $intPattern == 0b00110101 ) or 
    ( $intPattern == 0b00111001 ) or 
    ( $intPattern == 0b00111101 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000101 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100101 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10011001 ) or 
    ( $intPattern == 0b10011101 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) or 
    ( $intPattern == 0b10110001 ) or 
    ( $intPattern == 0b10110101 ) or 
    ( $intPattern == 0b10111001 ) or 
    ( $intPattern == 0b10111101 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000101 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b01000011 ) or 
    ( $intPattern == 0b01100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) or 
    ( $intPattern == 0b11000011 ) or 
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b01000111 ) or 
    ( $intPattern == 0b01100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) or 
    ( $intPattern == 0b11000111 ) or 
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b00011011 ) or 
    ( $intPattern == 0b01001011 ) or 
    ( $intPattern == 0b10001011 ) or 
    ( $intPattern == 0b10011011 ) or 
    ( $intPattern == 0b11001011 ) 
  ){
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,3,3,2);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) or 
    ( $intPattern == 0b11001111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,3,3,2);
        $varE01 = _varInterp2($varC4,$varC1,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00011110 ) or 
    ( $intPattern == 0b00110010 ) or 
    ( $intPattern == 0b00110110 ) or 
    ( $intPattern == 0b00111110 ) or 
    ( $intPattern == 0b01010110 ) or 
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp2($varC2,$varC1,3,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC2,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) or 
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = $varC3;
    $varE11 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC3;
        $varE01 = $varC3;
    } else {
        $varE00 = _varInterp2($varC3,$varC1,3,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC3,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b00011010 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011111 ) or 
    ( $intPattern == 0b01011111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,3,3,2);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) or 
    ( $intPattern == 0b10111011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,3,3,2);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111010 ) or 
    ( $intPattern == 0b10011010 ) or 
    ( $intPattern == 0b10111010 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) or 
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101010 ) or 
    ( $intPattern == 0b01101100 ) or 
    ( $intPattern == 0b01101110 ) or 
    ( $intPattern == 0b01111000 ) or 
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) or 
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) or 
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = $varC1;
        $varE10 = $varC1;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,3,1);
        $varE10 = _varInterp3($varC3,$varC7,$varC1,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) or 
    ( $intPattern == 0b11001010 ) or 
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010010 ) or 
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010001 ) or 
    ( $intPattern == 0b11010001 ) or 
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) or 
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) or 
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
    } else {
        $varE11 = _varInterp3($varC2,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b11010100 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC5,3,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC0,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01010101 ) or 
    ( $intPattern == 0b11010101 ) or 
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = $varC1;
        $varE11 = $varC1;
    } else {
        $varE01 = _varInterp2($varC1,$varC5,3,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC1,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC3;
    } else {
        $varE11 = _varInterp3($varC3,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) or 
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) or 
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
    } else {
        $varE10 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC2;
    } else {
        $varE10 = _varInterp3($varC2,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
    } else {
        $varE11 = _varInterp3($varC2,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101011 ) or 
    ( $intPattern == 0b01111011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC2;
    } else {
        $varE10 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b11110000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE10 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC7,3,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC0,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01110001 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE10 = $varC1;
        $varE11 = $varC1;
    } else {
        $varE10 = _varInterp2($varC1,$varC7,3,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC1,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
    } else {
        $varE10 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110010 ) or 
    ( $intPattern == 0b10110110 ) or 
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC0,3,3,2);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) or 
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) or 
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE10 = _varInterp3($varC3,$varC7,$varC0,3,3,2);
        $varE11 = _varInterp2($varC0,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
    } else {
        $varE10 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
    } else {
        $varE11 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC3;
    } else {
        $varE11 = _varInterp3($varC3,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE01 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
    } else {
        $varE11 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE10 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
    } else {
        $varE10 = _varInterp3($varC1,$varC3,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC2;
    } else {
        $varE10 = _varInterp3($varC2,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE10 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE10 = _varInterp2($varC2,$varC7,3,1);
        $varE11 = _varInterp3($varC5,$varC7,$varC2,3,3,2);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC3;
    } else {
        $varE11 = _varInterp3($varC3,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
    } else {
        $varE10 = _varInterp3($varC1,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE01 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC2;
    } else {
        $varE10 = _varInterp3($varC2,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
    } else {
        $varE11 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
    } else {
        $varE10 = _varInterp3($varC1,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
    } else {
        $varE11 = _varInterp3($varC1,$varC5,$varC7,14,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
    } else {
        $varE11 = _varInterp3($varC0,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp3($varC4,$varC3,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp3($varC4,$varC5,$varC7,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,14,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,14,1,1);
    }
  }
  #END LQ2x PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
  );
}

# standard LQ2x3 casepath
sub _arrLQ2x3 {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  my ( $varE20, $varE21,  ) = ( $varC4, $varC4, );
  #BEGIN LQ2x3 PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00011000 ) or 
    ( $intPattern == 0b00011100 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00111000 ) or 
    ( $intPattern == 0b00111100 ) or 
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000010 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000110 ) or 
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100010 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100110 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) or 
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10011000 ) or 
    ( $intPattern == 0b10011100 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100010 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100110 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10111000 ) or 
    ( $intPattern == 0b10111100 ) or 
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000010 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000110 ) or 
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100010 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
  } elsif (
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00011001 ) or 
    ( $intPattern == 0b00011101 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b00110001 ) or 
    ( $intPattern == 0b00110101 ) or 
    ( $intPattern == 0b00111001 ) or 
    ( $intPattern == 0b00111101 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000101 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100101 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10011001 ) or 
    ( $intPattern == 0b10011101 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) or 
    ( $intPattern == 0b10110001 ) or 
    ( $intPattern == 0b10110101 ) or 
    ( $intPattern == 0b10111001 ) or 
    ( $intPattern == 0b10111101 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000101 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b01000011 ) or 
    ( $intPattern == 0b01100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) or 
    ( $intPattern == 0b11000011 ) or 
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b01000111 ) or 
    ( $intPattern == 0b01100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) or 
    ( $intPattern == 0b11000111 ) or 
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC0,$varC1,15,1);
        $varE10 = _varInterp2($varC0,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b00011011 ) or 
    ( $intPattern == 0b01001011 ) or 
    ( $intPattern == 0b10001011 ) or 
    ( $intPattern == 0b10011011 ) or 
    ( $intPattern == 0b11001011 ) 
  ){
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC2,$varC1,15,1);
        $varE10 = _varInterp2($varC2,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,10,5,1);
        $varE01 = _varInterp2($varC0,$varC1,5,3);
        $varE10 = _varInterp2($varC0,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) or 
    ( $intPattern == 0b11001111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,10,5,1);
        $varE01 = _varInterp2($varC4,$varC1,5,3);
        $varE10 = _varInterp2($varC4,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00011110 ) or 
    ( $intPattern == 0b00110010 ) or 
    ( $intPattern == 0b00110110 ) or 
    ( $intPattern == 0b00111110 ) or 
    ( $intPattern == 0b01010110 ) or 
    ( $intPattern == 0b01110110 ) 
  ){
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC0,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE00 = _varInterp2($varC2,$varC1,5,3);
        $varE01 = _varInterp3($varC1,$varC5,$varC2,10,5,1);
        $varE11 = _varInterp2($varC2,$varC5,13,3);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) or 
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC3;
        $varE01 = $varC3;
        $varE11 = $varC3;
    } else {
        $varE00 = _varInterp2($varC3,$varC1,5,3);
        $varE01 = _varInterp3($varC1,$varC5,$varC3,10,5,1);
        $varE11 = _varInterp2($varC3,$varC5,13,3);
    }
  } elsif (
    ( $intPattern == 0b00011010 ) 
  ){
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC0,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC0,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00011111 ) or 
    ( $intPattern == 0b01011111 ) 
  ){
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE11 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,7,5,4);
        $varE01 = _varInterp2($varC0,$varC1,15,1);
        $varE10 = _varInterp2($varC0,$varC3,1,1);
        $varE20 = _varInterp2($varC0,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) or 
    ( $intPattern == 0b10111011 ) 
  ){
    $varE11 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
        $varE20 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,5,4);
        $varE01 = _varInterp2($varC2,$varC1,15,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
        $varE20 = _varInterp2($varC2,$varC3,13,3);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b00111010 ) or 
    ( $intPattern == 0b10011010 ) or 
    ( $intPattern == 0b10111010 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_E( $varC1, $varC5 )) {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,10,3,3);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_E( $varC1, $varC3 ))) {
        $varE01 = _varInterp2($varC2,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE01 = $varC2;
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC2,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE10 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) or 
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101010 ) or 
    ( $intPattern == 0b01101100 ) or 
    ( $intPattern == 0b01101110 ) or 
    ( $intPattern == 0b01111000 ) or 
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC0,$varC7,15,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) or 
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) or 
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = $varC1;
        $varE10 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,13,3);
        $varE10 = _varInterp2($varC1,$varC3,1,1);
        $varE20 = _varInterp3($varC7,$varC3,$varC1,7,5,4);
        $varE21 = _varInterp2($varC1,$varC7,15,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC0,$varC7,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC0,$varC1,15,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) or 
    ( $intPattern == 0b11001010 ) or 
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE11 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC4,$varC1,15,1);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010010 ) or 
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,15,1);
        $varE20 = _varInterp2($varC0,$varC7,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01010001 ) or 
    ( $intPattern == 0b11010001 ) or 
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE11 = _varInterp2($varC1,$varC5,15,1);
        $varE20 = _varInterp2($varC1,$varC7,15,1);
        $varE21 = _varInterp3($varC1,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) or 
    ( $intPattern == 0b11011110 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC7,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
    } else {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) or 
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
    } else {
        $varE21 = _varInterp3($varC2,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b11010100 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC5,13,3);
        $varE11 = _varInterp2($varC0,$varC5,1,1);
        $varE20 = _varInterp2($varC0,$varC7,15,1);
        $varE21 = _varInterp3($varC7,$varC5,$varC0,7,5,4);
    }
  } elsif (
    ( $intPattern == 0b01010101 ) or 
    ( $intPattern == 0b11010101 ) or 
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = $varC1;
        $varE11 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE01 = _varInterp2($varC1,$varC5,13,3);
        $varE11 = _varInterp2($varC1,$varC5,1,1);
        $varE20 = _varInterp2($varC1,$varC7,15,1);
        $varE21 = _varInterp3($varC7,$varC5,$varC1,7,5,4);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE10 = $varC3;
    $varE20 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC3;
    } else {
        $varE21 = _varInterp3($varC3,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC3;
        $varE01 = $varC3;
        $varE11 = $varC3;
    } else {
        $varE00 = _varInterp2($varC3,$varC1,15,1);
        $varE01 = _varInterp3($varC3,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC3,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) or 
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) or 
    ( $intPattern == 0b01011101 ) or 
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
    } else {
        $varE20 = _varInterp3($varC1,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
    } else {
        $varE21 = _varInterp3($varC1,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    $varE11 = $varC2;
    if (_boolYUV_E( $varC1, $varC5 )) {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,10,3,3);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_E( $varC1, $varC3 ))) {
        $varE01 = _varInterp2($varC2,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE01 = $varC2;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
    } else {
        $varE20 = _varInterp3($varC2,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
    } else {
        $varE21 = _varInterp3($varC2,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC2,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    $varE10 = $varC0;
    if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
    if ((_boolYUV_E( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = $varC0;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC0,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01101011 ) or 
    ( $intPattern == 0b01111011 ) 
  ){
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
        $varE21 = $varC2;
    } else {
        $varE20 = _varInterp3($varC2,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC2,$varC7,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC2,$varC1,15,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC4,$varC7,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b11110000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,13,3);
        $varE20 = _varInterp2($varC0,$varC7,9,7);
        $varE21 = _varInterp3($varC7,$varC5,$varC0,10,5,1);
    }
  } elsif (
    ( $intPattern == 0b01110001 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE11 = _varInterp2($varC1,$varC5,13,3);
        $varE20 = _varInterp2($varC1,$varC7,9,7);
        $varE21 = _varInterp3($varC7,$varC5,$varC1,10,5,1);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b11110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01110101 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
    } else {
        $varE21 = _varInterp3($varC1,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_E( $varC7, $varC5 )) {
        $varE21 = _varInterp3($varC1,$varC5,$varC7,10,3,3);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_E( $varC7, $varC3 ))) {
        $varE21 = _varInterp2($varC1,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE21 = $varC1;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
        $varE20 = $varC1;
    } else {
        $varE10 = _varInterp2($varC1,$varC3,15,1);
        $varE20 = _varInterp3($varC1,$varC3,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    $varE11 = $varC0;
    if (_boolYUV_E( $varC7, $varC5 )) {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_E( $varC7, $varC3 ))) {
        $varE21 = _varInterp2($varC0,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE21 = $varC0;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC0,$varC7,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC0,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    if (_boolYUV_E( $varC1, $varC5 )) {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,5,5);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_E( $varC1, $varC3 ))) {
        $varE01 = _varInterp2($varC4,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE01 = $varC4;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC4,$varC7,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE11 = $varC4;
    } else {
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110010 ) or 
    ( $intPattern == 0b10110110 ) or 
    ( $intPattern == 0b10111110 ) 
  ){
    $varE10 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE11 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
        $varE01 = _varInterp3($varC1,$varC5,$varC0,7,5,4);
        $varE11 = _varInterp2($varC0,$varC5,1,1);
        $varE21 = _varInterp2($varC0,$varC5,13,3);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
    if ((_boolYUV_E( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = $varC0;
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
        $varE11 = _varInterp2($varC0,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) or 
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) or 
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,13,3);
        $varE20 = _varInterp3($varC7,$varC3,$varC0,10,5,1);
        $varE21 = _varInterp2($varC0,$varC7,9,7);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) or 
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE21 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
    } else {
        $varE20 = _varInterp3($varC1,$varC3,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
        $varE20 = $varC2;
        $varE21 = $varC2;
    } else {
        $varE11 = _varInterp2($varC2,$varC5,15,1);
        $varE20 = _varInterp2($varC2,$varC7,15,1);
        $varE21 = _varInterp3($varC2,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = $varC3;
        $varE21 = $varC3;
    } else {
        $varE20 = _varInterp2($varC3,$varC7,15,1);
        $varE21 = _varInterp3($varC3,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    $varE10 = $varC0;
    if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,10,3,3);
    }
    if ((_boolYUV_E( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp2($varC0,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = $varC0;
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
        $varE20 = $varC2;
        $varE21 = $varC2;
    } else {
        $varE11 = _varInterp2($varC2,$varC5,15,1);
        $varE20 = _varInterp2($varC2,$varC7,15,1);
        $varE21 = _varInterp3($varC2,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC2,$varC1,15,1);
        $varE10 = _varInterp2($varC2,$varC3,15,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,10,3,3);
    }
    if ((_boolYUV_E( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp2($varC0,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = $varC0;
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,5,5);
    }
    if ((_boolYUV_E( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = _varInterp2($varC4,$varC1,15,1);
    }
    if ((_boolYUV_NE( $varC1, $varC5 ) and _boolYUV_NE( $varC1, $varC3 ))) {
        $varE00 = $varC4;
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC7,15,1);
        $varE21 = _varInterp3($varC4,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE10 = $varC4;
    } else {
        $varE10 = _varInterp2($varC4,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
        $varE11 = _varInterp2($varC4,$varC5,15,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
        $varE21 = _varInterp2($varC0,$varC7,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
    } else {
        $varE20 = _varInterp3($varC2,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC2,$varC1,15,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,15,1);
        $varE20 = _varInterp2($varC0,$varC7,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
        $varE20 = $varC2;
        $varE21 = $varC2;
    } else {
        $varE11 = _varInterp2($varC2,$varC5,13,3);
        $varE20 = _varInterp2($varC2,$varC7,9,7);
        $varE21 = _varInterp3($varC7,$varC5,$varC2,10,5,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
    } else {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC3;
    } else {
        $varE21 = _varInterp3($varC3,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
    } else {
        $varE20 = _varInterp3($varC1,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE11 = _varInterp2($varC1,$varC5,15,1);
        $varE21 = _varInterp3($varC1,$varC5,$varC7,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    if (_boolYUV_E( $varC7, $varC5 )) {
        $varE21 = _varInterp3($varC2,$varC5,$varC7,6,5,5);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_E( $varC7, $varC3 ))) {
        $varE21 = _varInterp2($varC2,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE21 = $varC2;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC2;
        $varE20 = $varC2;
    } else {
        $varE10 = _varInterp2($varC2,$varC3,15,1);
        $varE20 = _varInterp3($varC2,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC2;
    } else {
        $varE11 = _varInterp2($varC2,$varC5,15,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp3($varC2,$varC1,$varC3,6,5,5);
        $varE01 = _varInterp2($varC2,$varC1,15,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    if (_boolYUV_E( $varC7, $varC3 )) {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,6,5,5);
    }
    if ((_boolYUV_E( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = _varInterp2($varC0,$varC7,15,1);
    }
    if ((_boolYUV_NE( $varC7, $varC5 ) and _boolYUV_NE( $varC7, $varC3 ))) {
        $varE20 = $varC0;
    }
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,15,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE11 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE11 = _varInterp2($varC0,$varC5,15,1);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
    } else {
        $varE00 = _varInterp2($varC0,$varC1,15,1);
        $varE01 = _varInterp3($varC0,$varC1,$varC5,6,5,5);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
    } else {
        $varE21 = _varInterp3($varC4,$varC5,$varC7,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,10,3,3);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,10,3,3);
    }
  }
  #END LQ2x3 PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
    $varE20, $varE21, 
  );
}

# standard LQ2x4 casepath
sub _arrLQ2x4 {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  my ( $varE20, $varE21,  ) = ( $varC4, $varC4, );
  my ( $varE30, $varE31,  ) = ( $varC4, $varC4, );
  #BEGIN LQ2x4 PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00011000 ) or 
    ( $intPattern == 0b00011100 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00111000 ) or 
    ( $intPattern == 0b00111100 ) or 
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000010 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000110 ) or 
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100010 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100110 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) or 
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10011000 ) or 
    ( $intPattern == 0b10011100 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100010 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100110 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10111000 ) or 
    ( $intPattern == 0b10111100 ) or 
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000010 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000110 ) or 
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100010 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
  } elsif (
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00011001 ) or 
    ( $intPattern == 0b00011101 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b00110001 ) or 
    ( $intPattern == 0b00110101 ) or 
    ( $intPattern == 0b00111001 ) or 
    ( $intPattern == 0b00111101 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000101 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100101 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10011001 ) or 
    ( $intPattern == 0b10011101 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) or 
    ( $intPattern == 0b10110001 ) or 
    ( $intPattern == 0b10110101 ) or 
    ( $intPattern == 0b10111001 ) or 
    ( $intPattern == 0b10111101 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000101 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE30 = $varC1;
    $varE31 = $varC1;
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b01000011 ) or 
    ( $intPattern == 0b01100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) or 
    ( $intPattern == 0b11000011 ) or 
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b01000111 ) or 
    ( $intPattern == 0b01100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) or 
    ( $intPattern == 0b11000111 ) or 
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC0,$varC3,2,1,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b00011011 ) or 
    ( $intPattern == 0b01001011 ) or 
    ( $intPattern == 0b10001011 ) or 
    ( $intPattern == 0b10011011 ) or 
    ( $intPattern == 0b11001011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,9,7);
        $varE01 = _varInterp2($varC0,$varC1,1,1);
        $varE10 = _varInterp3($varC0,$varC3,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) or 
    ( $intPattern == 0b11001111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,9,7);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp3($varC4,$varC3,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00011110 ) or 
    ( $intPattern == 0b00110010 ) or 
    ( $intPattern == 0b00110110 ) or 
    ( $intPattern == 0b00111110 ) or 
    ( $intPattern == 0b01010110 ) or 
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC2,1,1);
        $varE01 = _varInterp2($varC1,$varC5,9,7);
        $varE11 = _varInterp3($varC2,$varC5,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) or 
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC3;
        $varE01 = $varC3;
        $varE11 = $varC3;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC5,9,7);
        $varE11 = _varInterp3($varC3,$varC5,$varC1,8,5,3);
    }
  } elsif (
    ( $intPattern == 0b00011010 ) 
  ){
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC0,$varC3,2,1,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00011111 ) or 
    ( $intPattern == 0b01011111 ) 
  ){
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,4,3,1);
        $varE10 = _varInterp3($varC0,$varC3,$varC1,3,3,2);
        $varE20 = _varInterp2($varC0,$varC3,5,3);
        $varE30 = _varInterp2($varC0,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) or 
    ( $intPattern == 0b10111011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    $varE21 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
        $varE20 = $varC2;
        $varE30 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,4,3,1);
        $varE10 = _varInterp3($varC2,$varC3,$varC1,3,3,2);
        $varE20 = _varInterp2($varC2,$varC3,5,3);
        $varE30 = _varInterp2($varC2,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111010 ) or 
    ( $intPattern == 0b10011010 ) or 
    ( $intPattern == 0b10111010 ) 
  ){
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC2,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE10 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) or 
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101010 ) or 
    ( $intPattern == 0b01101100 ) or 
    ( $intPattern == 0b01101110 ) or 
    ( $intPattern == 0b01111000 ) or 
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) or 
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) or 
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = $varC1;
    $varE11 = $varC1;
    $varE21 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = $varC1;
        $varE10 = $varC1;
        $varE20 = $varC1;
        $varE30 = $varC1;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,7,1);
        $varE10 = _varInterp2($varC1,$varC3,5,3);
        $varE20 = _varInterp3($varC1,$varC3,$varC7,3,3,2);
        $varE30 = _varInterp3($varC7,$varC3,$varC1,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC0,$varC3,2,1,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) or 
    ( $intPattern == 0b11001010 ) or 
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,7,1);
        $varE30 = _varInterp3($varC0,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE21 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,7,1);
        $varE30 = _varInterp3($varC4,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010010 ) or 
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010001 ) or 
    ( $intPattern == 0b11010001 ) or 
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE30 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE21 = _varInterp2($varC1,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) or 
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) or 
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = $varC2;
    $varE10 = $varC2;
    $varE20 = $varC2;
    $varE30 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE21 = _varInterp2($varC2,$varC5,7,1);
        $varE31 = _varInterp3($varC2,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC2,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b11010100 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC5,7,1);
        $varE11 = _varInterp2($varC0,$varC5,5,3);
        $varE21 = _varInterp3($varC0,$varC5,$varC7,3,3,2);
        $varE31 = _varInterp3($varC7,$varC5,$varC0,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010101 ) or 
    ( $intPattern == 0b11010101 ) or 
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = $varC1;
    $varE10 = $varC1;
    $varE20 = $varC1;
    $varE30 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE01 = $varC1;
        $varE11 = $varC1;
        $varE21 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE01 = _varInterp2($varC1,$varC5,7,1);
        $varE11 = _varInterp2($varC1,$varC5,5,3);
        $varE21 = _varInterp3($varC1,$varC5,$varC7,3,3,2);
        $varE31 = _varInterp3($varC7,$varC5,$varC1,4,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE20 = $varC3;
    $varE30 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC3;
        $varE31 = $varC3;
    } else {
        $varE21 = _varInterp2($varC3,$varC5,7,1);
        $varE31 = _varInterp3($varC3,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
        $varE11 = $varC3;
    } else {
        $varE01 = _varInterp3($varC1,$varC3,$varC5,2,1,1);
        $varE11 = _varInterp2($varC3,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) or 
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) or 
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
        $varE30 = $varC1;
    } else {
        $varE20 = _varInterp2($varC1,$varC3,7,1);
        $varE30 = _varInterp3($varC1,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE21 = _varInterp2($varC1,$varC5,7,1);
        $varE31 = _varInterp3($varC1,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,7,1);
        $varE30 = _varInterp3($varC0,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,7,1);
        $varE31 = _varInterp3($varC0,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
        $varE30 = $varC2;
    } else {
        $varE20 = _varInterp2($varC2,$varC3,7,1);
        $varE30 = _varInterp3($varC2,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE21 = _varInterp2($varC2,$varC5,7,1);
        $varE31 = _varInterp3($varC2,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC2,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,7,1);
        $varE30 = _varInterp3($varC0,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,7,1);
        $varE31 = _varInterp3($varC0,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,7,1);
        $varE30 = _varInterp3($varC0,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,7,1);
        $varE31 = _varInterp3($varC0,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101011 ) or 
    ( $intPattern == 0b01111011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    $varE21 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
        $varE30 = $varC2;
    } else {
        $varE20 = _varInterp2($varC2,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC2,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE21 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b11110000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp3($varC0,$varC5,$varC7,8,5,3);
        $varE30 = _varInterp2($varC0,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC5,9,7);
    }
  } elsif (
    ( $intPattern == 0b01110001 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
        $varE30 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE21 = _varInterp3($varC1,$varC5,$varC7,8,5,3);
        $varE30 = _varInterp2($varC1,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC5,9,7);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,7,1);
        $varE31 = _varInterp3($varC0,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,7,1);
        $varE31 = _varInterp3($varC0,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE30 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE21 = _varInterp2($varC1,$varC5,7,1);
        $varE31 = _varInterp3($varC1,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
        $varE30 = $varC1;
    } else {
        $varE20 = _varInterp2($varC1,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE21 = _varInterp2($varC1,$varC5,7,1);
        $varE31 = _varInterp3($varC1,$varC7,$varC5,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,7,1);
        $varE31 = _varInterp3($varC0,$varC7,$varC5,5,2,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE21 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE10 = $varC4;
    $varE21 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC3,$varC4,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE01 = _varInterp3($varC1,$varC4,$varC5,2,1,1);
        $varE11 = _varInterp2($varC4,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110010 ) or 
    ( $intPattern == 0b10110110 ) or 
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC5,$varC0,4,3,1);
        $varE11 = _varInterp3($varC0,$varC5,$varC1,3,3,2);
        $varE21 = _varInterp2($varC0,$varC5,5,3);
        $varE31 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = $varC2;
    $varE10 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC2;
        $varE11 = $varC2;
    } else {
        $varE01 = _varInterp3($varC2,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC2,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) or 
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) or 
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,8,5,3);
        $varE30 = _varInterp2($varC7,$varC3,9,7);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE21 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
        $varE30 = $varC1;
    } else {
        $varE20 = _varInterp2($varC1,$varC3,7,1);
        $varE30 = _varInterp3($varC1,$varC7,$varC3,5,2,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE30 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE21 = _varInterp2($varC2,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC2,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE30 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC3;
        $varE31 = $varC3;
    } else {
        $varE21 = _varInterp2($varC3,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC3,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,7,1);
        $varE30 = _varInterp3($varC0,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE30 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE21 = _varInterp2($varC2,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC2,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,7,1);
        $varE30 = _varInterp3($varC0,$varC7,$varC3,5,2,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE30 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC4,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,2,1,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC1;
    } else {
        $varE30 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,5,2,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC2;
    } else {
        $varE30 = _varInterp3($varC2,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC0,$varC1,$varC5,5,2,1);
        $varE11 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
        $varE30 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE21 = _varInterp3($varC2,$varC5,$varC7,8,5,3);
        $varE30 = _varInterp2($varC2,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC5,9,7);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC0;
    } else {
        $varE31 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE30 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC1;
    } else {
        $varE31 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC0;
    } else {
        $varE31 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE30 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC3;
    } else {
        $varE31 = _varInterp3($varC3,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
    } else {
        $varE01 = _varInterp3($varC3,$varC1,$varC5,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC1;
    } else {
        $varE30 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE21 = _varInterp2($varC1,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE01 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC2;
    } else {
        $varE30 = _varInterp3($varC2,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE21 = _varInterp2($varC2,$varC5,3,1);
        $varE31 = _varInterp3($varC7,$varC2,$varC5,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC0;
    } else {
        $varE31 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC1;
    } else {
        $varE30 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC1;
    } else {
        $varE31 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC7,$varC0,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC0;
    } else {
        $varE31 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE01 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE11 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE31 = $varC4;
    } else {
        $varE31 = _varInterp3($varC4,$varC5,$varC7,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
    } else {
        $varE01 = _varInterp3($varC4,$varC1,$varC5,6,1,1);
    }
  }
  #END LQ2x4 PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
    $varE20, $varE21, 
    $varE30, $varE31, 
  );
}

# standard LQ3x casepath
sub _arrLQ3x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01, $varE02,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12,  ) = ( $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22,  ) = ( $varC4, $varC4, $varC4, );
  #BEGIN LQ3x PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00011000 ) or 
    ( $intPattern == 0b00011100 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00111000 ) or 
    ( $intPattern == 0b00111100 ) or 
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000010 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000110 ) or 
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100010 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100110 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) or 
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10011000 ) or 
    ( $intPattern == 0b10011100 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100010 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100110 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10111000 ) or 
    ( $intPattern == 0b10111100 ) or 
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000010 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000110 ) or 
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100010 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
  } elsif (
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00011001 ) or 
    ( $intPattern == 0b00011101 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b00110001 ) or 
    ( $intPattern == 0b00110101 ) or 
    ( $intPattern == 0b00111001 ) or 
    ( $intPattern == 0b00111101 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000101 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100101 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10011001 ) or 
    ( $intPattern == 0b10011101 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) or 
    ( $intPattern == 0b10110001 ) or 
    ( $intPattern == 0b10110101 ) or 
    ( $intPattern == 0b10111001 ) or 
    ( $intPattern == 0b10111101 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000101 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b01000011 ) or 
    ( $intPattern == 0b01100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) or 
    ( $intPattern == 0b11000011 ) or 
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE02 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b01000111 ) or 
    ( $intPattern == 0b01100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) or 
    ( $intPattern == 0b11000111 ) or 
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE02 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,7,7,2);
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b00011011 ) or 
    ( $intPattern == 0b01001011 ) or 
    ( $intPattern == 0b10001011 ) or 
    ( $intPattern == 0b10011011 ) or 
    ( $intPattern == 0b11001011 ) 
  ){
    $varE02 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
        $varE10 = _varInterp2($varC2,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC0,3,1);
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) or 
    ( $intPattern == 0b11001111 ) 
  ){
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,3,1);
        $varE02 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp2($varC4,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00011110 ) or 
    ( $intPattern == 0b00110010 ) or 
    ( $intPattern == 0b00110110 ) or 
    ( $intPattern == 0b00111110 ) or 
    ( $intPattern == 0b01010110 ) or 
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE12 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
        $varE12 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE02 = $varC2;
        $varE12 = $varC2;
    } else {
        $varE00 = _varInterp2($varC2,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC2,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC2,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) or 
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC3;
        $varE01 = $varC3;
        $varE02 = $varC3;
        $varE12 = $varC3;
    } else {
        $varE00 = _varInterp2($varC3,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC3,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC3,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00011010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,7,7,2);
        $varE10 = _varInterp2($varC0,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE12 = $varC0;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
        $varE12 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00011111 ) or 
    ( $intPattern == 0b01011111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC3,$varC0,3,1);
        $varE20 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) or 
    ( $intPattern == 0b10111011 ) 
  ){
    $varE02 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
        $varE20 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC2,$varC1,3,1);
        $varE10 = _varInterp2($varC3,$varC2,3,1);
        $varE20 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111010 ) or 
    ( $intPattern == 0b10011010 ) or 
    ( $intPattern == 0b10111010 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
    } else {
        $varE02 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
        $varE10 = _varInterp2($varC2,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
    } else {
        $varE02 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) or 
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101010 ) or 
    ( $intPattern == 0b01101100 ) or 
    ( $intPattern == 0b01101110 ) or 
    ( $intPattern == 0b01111000 ) or 
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) or 
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) or 
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE22 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = $varC1;
        $varE10 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,3,1);
        $varE10 = _varInterp2($varC3,$varC1,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC1,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) 
  ){
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC0,7,7,2);
        $varE01 = _varInterp2($varC0,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) or 
    ( $intPattern == 0b11001010 ) or 
    ( $intPattern == 0b11001110 ) 
  ){
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE02 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010010 ) or 
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,7,1);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC0,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01010001 ) or 
    ( $intPattern == 0b11010001 ) or 
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC1;
        $varE21 = $varC1;
        $varE22 = $varC1;
    } else {
        $varE12 = _varInterp2($varC1,$varC5,7,1);
        $varE21 = _varInterp2($varC1,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC1,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) or 
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE21 = _varInterp2($varC0,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC0,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) or 
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC2;
    } else {
        $varE22 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
    } else {
        $varE02 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b11010100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE02 = $varC0;
        $varE12 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC5,3,1);
        $varE12 = _varInterp2($varC5,$varC0,3,1);
        $varE21 = _varInterp2($varC0,$varC7,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010101 ) or 
    ( $intPattern == 0b11010101 ) or 
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE20 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE02 = $varC1;
        $varE12 = $varC1;
        $varE21 = $varC1;
        $varE22 = $varC1;
    } else {
        $varE02 = _varInterp2($varC1,$varC5,3,1);
        $varE12 = _varInterp2($varC5,$varC1,3,1);
        $varE21 = _varInterp2($varC1,$varC7,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC3;
    } else {
        $varE22 = _varInterp3($varC3,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC3;
        $varE02 = $varC3;
        $varE12 = $varC3;
    } else {
        $varE01 = _varInterp2($varC3,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC3,7,7,2);
        $varE12 = _varInterp2($varC3,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) or 
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC0,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) or 
    ( $intPattern == 0b01011101 ) or 
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE21 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
    } else {
        $varE20 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC1;
    } else {
        $varE22 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
    } else {
        $varE02 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
    } else {
        $varE20 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC2;
    } else {
        $varE22 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
        $varE10 = _varInterp2($varC2,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
    } else {
        $varE02 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE12 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
        $varE12 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01101011 ) or 
    ( $intPattern == 0b01111011 ) 
  ){
    $varE02 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
        $varE21 = $varC2;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC2,7,7,2);
        $varE21 = _varInterp2($varC2,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b11110000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,3,1);
        $varE20 = _varInterp2($varC0,$varC7,3,1);
        $varE21 = _varInterp2($varC7,$varC0,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110001 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
        $varE22 = $varC1;
    } else {
        $varE12 = _varInterp2($varC1,$varC5,3,1);
        $varE20 = _varInterp2($varC1,$varC7,3,1);
        $varE21 = _varInterp2($varC7,$varC1,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
    } else {
        $varE02 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) or 
    ( $intPattern == 0b11110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110101 ) or 
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC1;
    } else {
        $varE22 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
    } else {
        $varE10 = _varInterp2($varC1,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC1,7,7,2);
        $varE21 = _varInterp2($varC1,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC1;
    } else {
        $varE22 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    $varE01 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
    } else {
        $varE02 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = $varC0;
    $varE11 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE12 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
        $varE12 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE11 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE21 = $varC4;
    } else {
        $varE20 = _varInterp3($varC3,$varC7,$varC4,7,7,2);
        $varE21 = _varInterp2($varC4,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE02 = _varInterp3($varC1,$varC5,$varC4,7,7,2);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110010 ) or 
    ( $intPattern == 0b10110110 ) or 
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE12 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE02 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp2($varC5,$varC0,3,1);
        $varE22 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
    } else {
        $varE02 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC3;
    } else {
        $varE02 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE12 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
        $varE12 = _varInterp2($varC0,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE01 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) or 
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) or 
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,3,1);
        $varE20 = _varInterp2($varC3,$varC7,1,1);
        $varE21 = _varInterp2($varC7,$varC0,3,1);
        $varE22 = _varInterp2($varC0,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) or 
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
    } else {
        $varE20 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE02 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC2;
        $varE21 = $varC2;
        $varE22 = $varC2;
    } else {
        $varE12 = _varInterp2($varC2,$varC5,7,1);
        $varE21 = _varInterp2($varC2,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC2,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE20 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC3;
        $varE22 = $varC3;
    } else {
        $varE21 = _varInterp2($varC3,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC3,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC3;
    } else {
        $varE02 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,7,1);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC0,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
    } else {
        $varE02 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE02 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC2;
        $varE21 = $varC2;
        $varE22 = $varC2;
    } else {
        $varE12 = _varInterp2($varC2,$varC5,7,1);
        $varE21 = _varInterp2($varC2,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC2,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
        $varE10 = _varInterp2($varC2,$varC3,7,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
    } else {
        $varE20 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,7,1);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC0,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE11 = $varC4;
    $varE20 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE21 = $varC4;
        $varE22 = $varC4;
    } else {
        $varE21 = _varInterp2($varC4,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC4,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC4,7,7,2);
        $varE10 = _varInterp2($varC4,$varC3,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE12 = $varC4;
    } else {
        $varE01 = _varInterp2($varC4,$varC1,7,1);
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
        $varE12 = _varInterp2($varC4,$varC5,7,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
        $varE21 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE02 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
    } else {
        $varE20 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,7,1);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC0,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
    } else {
        $varE02 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE02 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC2;
        $varE20 = $varC2;
        $varE21 = $varC2;
        $varE22 = $varC2;
    } else {
        $varE12 = _varInterp2($varC2,$varC5,3,1);
        $varE20 = _varInterp2($varC2,$varC7,3,1);
        $varE21 = _varInterp2($varC7,$varC2,3,1);
        $varE22 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC3;
    } else {
        $varE22 = _varInterp3($varC3,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC3;
    } else {
        $varE02 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE21 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
    } else {
        $varE20 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC1;
        $varE22 = $varC1;
    } else {
        $varE12 = _varInterp2($varC1,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC1,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE02 = $varC2;
    $varE11 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC2;
        $varE20 = $varC2;
        $varE21 = $varC2;
    } else {
        $varE10 = _varInterp2($varC2,$varC3,7,1);
        $varE20 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
        $varE21 = _varInterp2($varC2,$varC7,7,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC2;
        $varE22 = $varC2;
    } else {
        $varE12 = _varInterp2($varC2,$varC5,7,1);
        $varE22 = _varInterp3($varC5,$varC7,$varC2,7,7,2);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
    } else {
        $varE00 = _varInterp3($varC1,$varC3,$varC2,7,7,2);
        $varE01 = _varInterp2($varC2,$varC1,7,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = $varC0;
    $varE11 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE10 = $varC0;
        $varE20 = $varC0;
    } else {
        $varE10 = _varInterp2($varC0,$varC3,7,1);
        $varE20 = _varInterp3($varC3,$varC7,$varC0,7,7,2);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE12 = $varC0;
        $varE21 = $varC0;
        $varE22 = $varC0;
    } else {
        $varE12 = _varInterp2($varC0,$varC5,7,1);
        $varE21 = _varInterp2($varC0,$varC7,7,1);
        $varE22 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE01 = $varC0;
        $varE02 = $varC0;
    } else {
        $varE01 = _varInterp2($varC0,$varC1,7,1);
        $varE02 = _varInterp3($varC1,$varC5,$varC0,7,7,2);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
    } else {
        $varE20 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC4;
    } else {
        $varE22 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
    } else {
        $varE02 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  }
  #END LQ3x PATTERNS
  return (
    $varE00, $varE01, $varE02, 
    $varE10, $varE11, $varE12, 
    $varE20, $varE21, $varE22, 
  );
}

# standard LQ4x casepath
sub _arrLQ4x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8,
    $intPattern
  )=@_;
  my ( $varE00, $varE01, $varE02, $varE03,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  my ( $varE10, $varE11, $varE12, $varE13,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  my ( $varE20, $varE21, $varE22, $varE23,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  my ( $varE30, $varE31, $varE32, $varE33,  ) = ( $varC4, $varC4, $varC4, $varC4, );
  #BEGIN LQ4x PATTERNS
  if (
    ( $intPattern == 0b00000000 ) or 
    ( $intPattern == 0b00000010 ) or 
    ( $intPattern == 0b00000100 ) or 
    ( $intPattern == 0b00000110 ) or 
    ( $intPattern == 0b00001000 ) or 
    ( $intPattern == 0b00001100 ) or 
    ( $intPattern == 0b00010000 ) or 
    ( $intPattern == 0b00010100 ) or 
    ( $intPattern == 0b00011000 ) or 
    ( $intPattern == 0b00011100 ) or 
    ( $intPattern == 0b00100000 ) or 
    ( $intPattern == 0b00100010 ) or 
    ( $intPattern == 0b00100100 ) or 
    ( $intPattern == 0b00100110 ) or 
    ( $intPattern == 0b00101000 ) or 
    ( $intPattern == 0b00101100 ) or 
    ( $intPattern == 0b00110000 ) or 
    ( $intPattern == 0b00110100 ) or 
    ( $intPattern == 0b00111000 ) or 
    ( $intPattern == 0b00111100 ) or 
    ( $intPattern == 0b01000000 ) or 
    ( $intPattern == 0b01000010 ) or 
    ( $intPattern == 0b01000100 ) or 
    ( $intPattern == 0b01000110 ) or 
    ( $intPattern == 0b01100000 ) or 
    ( $intPattern == 0b01100010 ) or 
    ( $intPattern == 0b01100100 ) or 
    ( $intPattern == 0b01100110 ) or 
    ( $intPattern == 0b10000000 ) or 
    ( $intPattern == 0b10000010 ) or 
    ( $intPattern == 0b10000100 ) or 
    ( $intPattern == 0b10000110 ) or 
    ( $intPattern == 0b10001000 ) or 
    ( $intPattern == 0b10001100 ) or 
    ( $intPattern == 0b10010000 ) or 
    ( $intPattern == 0b10010100 ) or 
    ( $intPattern == 0b10011000 ) or 
    ( $intPattern == 0b10011100 ) or 
    ( $intPattern == 0b10100000 ) or 
    ( $intPattern == 0b10100010 ) or 
    ( $intPattern == 0b10100100 ) or 
    ( $intPattern == 0b10100110 ) or 
    ( $intPattern == 0b10101000 ) or 
    ( $intPattern == 0b10101100 ) or 
    ( $intPattern == 0b10110000 ) or 
    ( $intPattern == 0b10110100 ) or 
    ( $intPattern == 0b10111000 ) or 
    ( $intPattern == 0b10111100 ) or 
    ( $intPattern == 0b11000000 ) or 
    ( $intPattern == 0b11000010 ) or 
    ( $intPattern == 0b11000100 ) or 
    ( $intPattern == 0b11000110 ) or 
    ( $intPattern == 0b11100000 ) or 
    ( $intPattern == 0b11100010 ) or 
    ( $intPattern == 0b11100100 ) or 
    ( $intPattern == 0b11100110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
  } elsif (
    ( $intPattern == 0b00000001 ) or 
    ( $intPattern == 0b00000101 ) or 
    ( $intPattern == 0b00001001 ) or 
    ( $intPattern == 0b00001101 ) or 
    ( $intPattern == 0b00010001 ) or 
    ( $intPattern == 0b00010101 ) or 
    ( $intPattern == 0b00011001 ) or 
    ( $intPattern == 0b00011101 ) or 
    ( $intPattern == 0b00100001 ) or 
    ( $intPattern == 0b00100101 ) or 
    ( $intPattern == 0b00101001 ) or 
    ( $intPattern == 0b00101101 ) or 
    ( $intPattern == 0b00110001 ) or 
    ( $intPattern == 0b00110101 ) or 
    ( $intPattern == 0b00111001 ) or 
    ( $intPattern == 0b00111101 ) or 
    ( $intPattern == 0b01000001 ) or 
    ( $intPattern == 0b01000101 ) or 
    ( $intPattern == 0b01100001 ) or 
    ( $intPattern == 0b01100101 ) or 
    ( $intPattern == 0b10000001 ) or 
    ( $intPattern == 0b10000101 ) or 
    ( $intPattern == 0b10001001 ) or 
    ( $intPattern == 0b10001101 ) or 
    ( $intPattern == 0b10010001 ) or 
    ( $intPattern == 0b10010101 ) or 
    ( $intPattern == 0b10011001 ) or 
    ( $intPattern == 0b10011101 ) or 
    ( $intPattern == 0b10100001 ) or 
    ( $intPattern == 0b10100101 ) or 
    ( $intPattern == 0b10101001 ) or 
    ( $intPattern == 0b10101101 ) or 
    ( $intPattern == 0b10110001 ) or 
    ( $intPattern == 0b10110101 ) or 
    ( $intPattern == 0b10111001 ) or 
    ( $intPattern == 0b10111101 ) or 
    ( $intPattern == 0b11000001 ) or 
    ( $intPattern == 0b11000101 ) or 
    ( $intPattern == 0b11100001 ) or 
    ( $intPattern == 0b11100101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE23 = $varC1;
    $varE30 = $varC1;
    $varE31 = $varC1;
    $varE32 = $varC1;
    $varE33 = $varC1;
  } elsif (
    ( $intPattern == 0b00000011 ) or 
    ( $intPattern == 0b00100011 ) or 
    ( $intPattern == 0b01000011 ) or 
    ( $intPattern == 0b01100011 ) or 
    ( $intPattern == 0b10000011 ) or 
    ( $intPattern == 0b10100011 ) or 
    ( $intPattern == 0b11000011 ) or 
    ( $intPattern == 0b11100011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
  } elsif (
    ( $intPattern == 0b00000111 ) or 
    ( $intPattern == 0b00100111 ) or 
    ( $intPattern == 0b01000111 ) or 
    ( $intPattern == 0b01100111 ) or 
    ( $intPattern == 0b10000111 ) or 
    ( $intPattern == 0b10100111 ) or 
    ( $intPattern == 0b11000111 ) or 
    ( $intPattern == 0b11100111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE02 = $varC3;
    $varE03 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE13 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    $varE23 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    $varE32 = $varC3;
    $varE33 = $varC3;
  } elsif (
    ( $intPattern == 0b00001010 ) or 
    ( $intPattern == 0b10001010 ) 
  ){
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC0,$varC1,1,1);
        $varE10 = _varInterp2($varC0,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001011 ) or 
    ( $intPattern == 0b00011011 ) or 
    ( $intPattern == 0b01001011 ) or 
    ( $intPattern == 0b10001011 ) or 
    ( $intPattern == 0b10011011 ) or 
    ( $intPattern == 0b11001011 ) 
  ){
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001110 ) or 
    ( $intPattern == 0b10001110 ) 
  ){
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE10 = $varC0;
        $varE11 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC3,5,3);
        $varE02 = _varInterp2($varC1,$varC0,3,1);
        $varE03 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp3($varC3,$varC0,$varC1,2,1,1);
        $varE11 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00001111 ) or 
    ( $intPattern == 0b10001111 ) or 
    ( $intPattern == 0b11001111 ) 
  ){
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE10 = $varC4;
        $varE11 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC3,5,3);
        $varE02 = _varInterp2($varC1,$varC4,3,1);
        $varE03 = _varInterp2($varC4,$varC1,3,1);
        $varE10 = _varInterp3($varC3,$varC1,$varC4,2,1,1);
        $varE11 = _varInterp3($varC4,$varC1,$varC3,6,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010010 ) or 
    ( $intPattern == 0b00010110 ) or 
    ( $intPattern == 0b00011110 ) or 
    ( $intPattern == 0b00110010 ) or 
    ( $intPattern == 0b00110110 ) or 
    ( $intPattern == 0b00111110 ) or 
    ( $intPattern == 0b01010110 ) or 
    ( $intPattern == 0b01110110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010011 ) or 
    ( $intPattern == 0b00110011 ) 
  ){
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE02 = $varC2;
        $varE03 = $varC2;
        $varE12 = $varC2;
        $varE13 = $varC2;
    } else {
        $varE00 = _varInterp2($varC2,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC2,3,1);
        $varE02 = _varInterp2($varC1,$varC5,5,3);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC2,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp3($varC5,$varC1,$varC2,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00010111 ) or 
    ( $intPattern == 0b00110111 ) or 
    ( $intPattern == 0b01110111 ) 
  ){
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    $varE23 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    $varE32 = $varC3;
    $varE33 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE00 = $varC3;
        $varE01 = $varC3;
        $varE02 = $varC3;
        $varE03 = $varC3;
        $varE12 = $varC3;
        $varE13 = $varC3;
    } else {
        $varE00 = _varInterp2($varC3,$varC1,3,1);
        $varE01 = _varInterp2($varC1,$varC3,3,1);
        $varE02 = _varInterp2($varC1,$varC5,5,3);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC3,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp3($varC5,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011010 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC0,$varC1,1,1);
        $varE10 = _varInterp2($varC0,$varC3,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00011111 ) or 
    ( $intPattern == 0b01011111 ) 
  ){
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b00101010 ) or 
    ( $intPattern == 0b10101010 ) 
  ){
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
        $varE11 = $varC0;
        $varE20 = $varC0;
        $varE30 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp3($varC1,$varC0,$varC3,2,1,1);
        $varE10 = _varInterp2($varC3,$varC1,5,3);
        $varE11 = _varInterp3($varC0,$varC1,$varC3,6,1,1);
        $varE20 = _varInterp2($varC3,$varC0,3,1);
        $varE30 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101011 ) or 
    ( $intPattern == 0b10101011 ) or 
    ( $intPattern == 0b10111011 ) 
  ){
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
        $varE11 = $varC2;
        $varE20 = $varC2;
        $varE30 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp3($varC1,$varC2,$varC3,2,1,1);
        $varE10 = _varInterp2($varC3,$varC1,5,3);
        $varE11 = _varInterp3($varC2,$varC1,$varC3,6,1,1);
        $varE20 = _varInterp2($varC3,$varC2,3,1);
        $varE30 = _varInterp2($varC2,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101110 ) or 
    ( $intPattern == 0b10101110 ) 
  ){
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b00101111 ) or 
    ( $intPattern == 0b10101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE03 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b00111010 ) or 
    ( $intPattern == 0b10011010 ) or 
    ( $intPattern == 0b10111010 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE03 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00111011 ) 
  ){
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
        $varE03 = $varC2;
        $varE13 = $varC2;
    } else {
        $varE02 = _varInterp2($varC2,$varC1,3,1);
        $varE03 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC2,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b00111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001000 ) or 
    ( $intPattern == 0b01001100 ) or 
    ( $intPattern == 0b01101000 ) or 
    ( $intPattern == 0b01101010 ) or 
    ( $intPattern == 0b01101100 ) or 
    ( $intPattern == 0b01101110 ) or 
    ( $intPattern == 0b01111000 ) or 
    ( $intPattern == 0b01111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001001 ) or 
    ( $intPattern == 0b01001101 ) or 
    ( $intPattern == 0b01101001 ) or 
    ( $intPattern == 0b01101101 ) or 
    ( $intPattern == 0b01111101 ) 
  ){
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE22 = $varC1;
    $varE23 = $varC1;
    $varE32 = $varC1;
    $varE33 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE00 = $varC1;
        $varE10 = $varC1;
        $varE20 = $varC1;
        $varE21 = $varC1;
        $varE30 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,3,1);
        $varE10 = _varInterp2($varC3,$varC1,3,1);
        $varE20 = _varInterp2($varC3,$varC7,5,3);
        $varE21 = _varInterp3($varC1,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp3($varC7,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001010 ) 
  ){
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC0,$varC1,1,1);
        $varE10 = _varInterp2($varC0,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b01001110 ) or 
    ( $intPattern == 0b11001010 ) or 
    ( $intPattern == 0b11001110 ) 
  ){
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC0,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b01001111 ) 
  ){
    $varE02 = $varC4;
    $varE03 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC4,$varC3,3,1);
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC4,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010000 ) or 
    ( $intPattern == 0b11010000 ) or 
    ( $intPattern == 0b11010010 ) or 
    ( $intPattern == 0b11011000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,1,1);
        $varE32 = _varInterp2($varC0,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010001 ) or 
    ( $intPattern == 0b11010001 ) or 
    ( $intPattern == 0b11011001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE30 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE23 = _varInterp2($varC1,$varC5,1,1);
        $varE32 = _varInterp2($varC1,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010010 ) or 
    ( $intPattern == 0b11010110 ) or 
    ( $intPattern == 0b11011110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,1,1);
        $varE32 = _varInterp2($varC0,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010011 ) or 
    ( $intPattern == 0b01110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC2;
        $varE32 = $varC2;
        $varE33 = $varC2;
    } else {
        $varE23 = _varInterp2($varC2,$varC5,3,1);
        $varE32 = _varInterp2($varC2,$varC7,3,1);
        $varE33 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
        $varE03 = $varC2;
        $varE13 = $varC2;
    } else {
        $varE02 = _varInterp2($varC2,$varC1,3,1);
        $varE03 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC2,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01010100 ) or 
    ( $intPattern == 0b11010100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE03 = $varC0;
        $varE13 = $varC0;
        $varE22 = $varC0;
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE03 = _varInterp2($varC0,$varC5,3,1);
        $varE13 = _varInterp2($varC5,$varC0,3,1);
        $varE22 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp2($varC5,$varC7,5,3);
        $varE32 = _varInterp3($varC7,$varC0,$varC5,2,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010101 ) or 
    ( $intPattern == 0b11010101 ) or 
    ( $intPattern == 0b11011101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE30 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE03 = $varC1;
        $varE13 = $varC1;
        $varE22 = $varC1;
        $varE23 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE03 = _varInterp2($varC1,$varC5,3,1);
        $varE13 = _varInterp2($varC5,$varC1,3,1);
        $varE22 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp2($varC5,$varC7,5,3);
        $varE32 = _varInterp3($varC7,$varC1,$varC5,2,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01010111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC3;
        $varE32 = $varC3;
        $varE33 = $varC3;
    } else {
        $varE23 = _varInterp2($varC3,$varC5,3,1);
        $varE32 = _varInterp2($varC3,$varC7,3,1);
        $varE33 = _varInterp3($varC3,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC3;
        $varE03 = $varC3;
        $varE13 = $varC3;
    } else {
        $varE02 = _varInterp2($varC1,$varC3,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC3,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011000 ) or 
    ( $intPattern == 0b11111000 ) or 
    ( $intPattern == 0b11111010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,1,1);
        $varE32 = _varInterp2($varC0,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011001 ) or 
    ( $intPattern == 0b01011101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
        $varE30 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE20 = _varInterp2($varC1,$varC3,3,1);
        $varE30 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC1,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE23 = _varInterp2($varC1,$varC5,3,1);
        $varE32 = _varInterp2($varC1,$varC7,3,1);
        $varE33 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011010 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC0,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,3,1);
        $varE32 = _varInterp2($varC0,$varC7,3,1);
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE03 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011011 ) 
  ){
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
        $varE30 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE20 = _varInterp2($varC2,$varC3,3,1);
        $varE30 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC2,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC2;
        $varE32 = $varC2;
        $varE33 = $varC2;
    } else {
        $varE23 = _varInterp2($varC2,$varC5,3,1);
        $varE32 = _varInterp2($varC2,$varC7,3,1);
        $varE33 = _varInterp3($varC2,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
        $varE03 = $varC2;
        $varE13 = $varC2;
    } else {
        $varE02 = _varInterp2($varC2,$varC1,3,1);
        $varE03 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC2,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC0,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,3,1);
        $varE32 = _varInterp2($varC0,$varC7,3,1);
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01011110 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC0,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,3,1);
        $varE32 = _varInterp2($varC0,$varC7,3,1);
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101011 ) or 
    ( $intPattern == 0b01111011 ) 
  ){
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC2;
        $varE30 = $varC2;
        $varE31 = $varC2;
    } else {
        $varE20 = _varInterp2($varC2,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC2,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b01101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE03 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110000 ) or 
    ( $intPattern == 0b11110000 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC0;
        $varE23 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE22 = _varInterp3($varC0,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp3($varC5,$varC0,$varC7,2,1,1);
        $varE30 = _varInterp2($varC0,$varC7,3,1);
        $varE31 = _varInterp2($varC7,$varC0,3,1);
        $varE32 = _varInterp2($varC7,$varC5,5,3);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110001 ) or 
    ( $intPattern == 0b11110001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC1;
        $varE23 = $varC1;
        $varE30 = $varC1;
        $varE31 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE22 = _varInterp3($varC1,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp3($varC5,$varC1,$varC7,2,1,1);
        $varE30 = _varInterp2($varC1,$varC7,3,1);
        $varE31 = _varInterp2($varC7,$varC1,3,1);
        $varE32 = _varInterp2($varC7,$varC5,5,3);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,3,1);
        $varE32 = _varInterp2($varC0,$varC7,3,1);
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE03 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,3,1);
        $varE32 = _varInterp2($varC0,$varC7,3,1);
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE30 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE23 = _varInterp2($varC1,$varC5,3,1);
        $varE32 = _varInterp2($varC1,$varC7,3,1);
        $varE33 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
        $varE30 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE20 = _varInterp2($varC1,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC1,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE23 = _varInterp2($varC1,$varC5,3,1);
        $varE32 = _varInterp2($varC1,$varC7,3,1);
        $varE33 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111010 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,3,1);
        $varE32 = _varInterp2($varC0,$varC7,3,1);
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE03 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b01111110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b01111111 ) 
  ){
    $varE01 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC4;
        $varE30 = $varC4;
        $varE31 = $varC4;
    } else {
        $varE20 = _varInterp2($varC3,$varC4,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC4,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC4;
        $varE03 = $varC4;
        $varE13 = $varC4;
    } else {
        $varE02 = _varInterp2($varC1,$varC4,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC4,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b10010010 ) or 
    ( $intPattern == 0b10010110 ) or 
    ( $intPattern == 0b10110010 ) or 
    ( $intPattern == 0b10110110 ) or 
    ( $intPattern == 0b10111110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE12 = $varC0;
        $varE13 = $varC0;
        $varE23 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE02 = _varInterp3($varC1,$varC0,$varC5,2,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE12 = _varInterp3($varC0,$varC1,$varC5,6,1,1);
        $varE13 = _varInterp2($varC5,$varC1,5,3);
        $varE23 = _varInterp2($varC5,$varC0,3,1);
        $varE33 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010011 ) or 
    ( $intPattern == 0b10110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC2;
        $varE03 = $varC2;
        $varE13 = $varC2;
    } else {
        $varE02 = _varInterp2($varC2,$varC1,3,1);
        $varE03 = _varInterp3($varC2,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC2,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b10010111 ) or 
    ( $intPattern == 0b10110111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE02 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE13 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    $varE23 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    $varE32 = $varC3;
    $varE33 = $varC3;
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC3;
    } else {
        $varE03 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011110 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b10011111 ) 
  ){
    $varE02 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b10111111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11001000 ) or 
    ( $intPattern == 0b11001100 ) or 
    ( $intPattern == 0b11101000 ) or 
    ( $intPattern == 0b11101100 ) or 
    ( $intPattern == 0b11101110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE21 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE20 = _varInterp3($varC3,$varC0,$varC7,2,1,1);
        $varE21 = _varInterp3($varC0,$varC3,$varC7,6,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC7,$varC3,5,3);
        $varE32 = _varInterp2($varC7,$varC0,3,1);
        $varE33 = _varInterp2($varC0,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11001001 ) or 
    ( $intPattern == 0b11001101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE23 = $varC1;
    $varE32 = $varC1;
    $varE33 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC1;
        $varE30 = $varC1;
        $varE31 = $varC1;
    } else {
        $varE20 = _varInterp2($varC1,$varC3,3,1);
        $varE30 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC1,$varC7,3,1);
    }
  } elsif (
    ( $intPattern == 0b11010011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC2;
        $varE32 = $varC2;
        $varE33 = $varC2;
    } else {
        $varE23 = _varInterp2($varC2,$varC5,1,1);
        $varE32 = _varInterp2($varC2,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11010111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE02 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE13 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC3;
        $varE32 = $varC3;
        $varE33 = $varC3;
    } else {
        $varE23 = _varInterp2($varC3,$varC5,1,1);
        $varE32 = _varInterp2($varC3,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC3;
    } else {
        $varE03 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011010 ) 
  ){
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC0,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,1,1);
        $varE32 = _varInterp2($varC0,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE03 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11011011 ) 
  ){
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE30 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC2;
        $varE32 = $varC2;
        $varE33 = $varC2;
    } else {
        $varE23 = _varInterp2($varC2,$varC5,1,1);
        $varE32 = _varInterp2($varC2,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,3,1);
        $varE30 = _varInterp3($varC0,$varC3,$varC7,2,1,1);
        $varE31 = _varInterp2($varC0,$varC7,3,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,1,1);
        $varE32 = _varInterp2($varC0,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11011111 ) 
  ){
    $varE02 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE30 = $varC4;
    $varE31 = $varC4;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC4;
        $varE32 = $varC4;
        $varE33 = $varC4;
    } else {
        $varE23 = _varInterp2($varC4,$varC5,1,1);
        $varE32 = _varInterp2($varC4,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
        $varE01 = $varC4;
        $varE10 = $varC4;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC4,1,1);
        $varE10 = _varInterp2($varC3,$varC4,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101001 ) or 
    ( $intPattern == 0b11101101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE23 = $varC1;
    $varE31 = $varC1;
    $varE32 = $varC1;
    $varE33 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC1;
    } else {
        $varE30 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101010 ) 
  ){
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    $varE33 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC0;
        $varE01 = $varC0;
        $varE10 = $varC0;
    } else {
        $varE00 = _varInterp3($varC0,$varC1,$varC3,2,1,1);
        $varE01 = _varInterp2($varC0,$varC1,3,1);
        $varE10 = _varInterp2($varC0,$varC3,3,1);
    }
  } elsif (
    ( $intPattern == 0b11101011 ) 
  ){
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE23 = $varC2;
    $varE31 = $varC2;
    $varE32 = $varC2;
    $varE33 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC2;
    } else {
        $varE30 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b11101111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE03 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    $varE33 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110010 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC0;
        $varE32 = $varC0;
        $varE33 = $varC0;
    } else {
        $varE23 = _varInterp2($varC0,$varC5,1,1);
        $varE32 = _varInterp2($varC0,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,3,1);
        $varE03 = _varInterp3($varC0,$varC1,$varC5,2,1,1);
        $varE13 = _varInterp2($varC0,$varC5,3,1);
    }
  } elsif (
    ( $intPattern == 0b11110011 ) 
  ){
    $varE00 = $varC2;
    $varE01 = $varC2;
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE10 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE22 = $varC2;
        $varE23 = $varC2;
        $varE30 = $varC2;
        $varE31 = $varC2;
        $varE32 = $varC2;
        $varE33 = $varC2;
    } else {
        $varE22 = _varInterp3($varC2,$varC5,$varC7,6,1,1);
        $varE23 = _varInterp3($varC5,$varC2,$varC7,2,1,1);
        $varE30 = _varInterp2($varC2,$varC7,3,1);
        $varE31 = _varInterp2($varC7,$varC2,3,1);
        $varE32 = _varInterp2($varC7,$varC5,5,3);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC0;
    } else {
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE23 = $varC1;
    $varE30 = $varC1;
    $varE31 = $varC1;
    $varE32 = $varC1;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC1;
    } else {
        $varE33 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE20 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE30 = $varC0;
    $varE31 = $varC0;
    $varE32 = $varC0;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC0;
    } else {
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b11110111 ) 
  ){
    $varE00 = $varC3;
    $varE01 = $varC3;
    $varE02 = $varC3;
    $varE10 = $varC3;
    $varE11 = $varC3;
    $varE12 = $varC3;
    $varE13 = $varC3;
    $varE20 = $varC3;
    $varE21 = $varC3;
    $varE22 = $varC3;
    $varE23 = $varC3;
    $varE30 = $varC3;
    $varE31 = $varC3;
    $varE32 = $varC3;
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC3;
    } else {
        $varE33 = _varInterp3($varC3,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC3;
    } else {
        $varE03 = _varInterp3($varC3,$varC1,$varC5,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111001 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE31 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC1;
    } else {
        $varE30 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC1;
        $varE32 = $varC1;
        $varE33 = $varC1;
    } else {
        $varE23 = _varInterp2($varC1,$varC5,1,1);
        $varE32 = _varInterp2($varC1,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111011 ) 
  ){
    $varE02 = $varC2;
    $varE03 = $varC2;
    $varE11 = $varC2;
    $varE12 = $varC2;
    $varE13 = $varC2;
    $varE20 = $varC2;
    $varE21 = $varC2;
    $varE22 = $varC2;
    $varE31 = $varC2;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC2;
    } else {
        $varE30 = _varInterp3($varC2,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE23 = $varC2;
        $varE32 = $varC2;
        $varE33 = $varC2;
    } else {
        $varE23 = _varInterp2($varC2,$varC5,1,1);
        $varE32 = _varInterp2($varC2,$varC7,1,1);
        $varE33 = _varInterp2($varC5,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC2;
        $varE01 = $varC2;
        $varE10 = $varC2;
    } else {
        $varE00 = _varInterp2($varC1,$varC3,1,1);
        $varE01 = _varInterp2($varC1,$varC2,1,1);
        $varE10 = _varInterp2($varC2,$varC3,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111100 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE02 = $varC0;
    $varE03 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE13 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC0;
    } else {
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111101 ) 
  ){
    $varE00 = $varC1;
    $varE01 = $varC1;
    $varE02 = $varC1;
    $varE03 = $varC1;
    $varE10 = $varC1;
    $varE11 = $varC1;
    $varE12 = $varC1;
    $varE13 = $varC1;
    $varE20 = $varC1;
    $varE21 = $varC1;
    $varE22 = $varC1;
    $varE23 = $varC1;
    $varE31 = $varC1;
    $varE32 = $varC1;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC1;
    } else {
        $varE30 = _varInterp3($varC1,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC1;
    } else {
        $varE33 = _varInterp3($varC1,$varC5,$varC7,2,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111110 ) 
  ){
    $varE00 = $varC0;
    $varE01 = $varC0;
    $varE10 = $varC0;
    $varE11 = $varC0;
    $varE12 = $varC0;
    $varE21 = $varC0;
    $varE22 = $varC0;
    $varE23 = $varC0;
    $varE32 = $varC0;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE20 = $varC0;
        $varE30 = $varC0;
        $varE31 = $varC0;
    } else {
        $varE20 = _varInterp2($varC0,$varC3,1,1);
        $varE30 = _varInterp2($varC3,$varC7,1,1);
        $varE31 = _varInterp2($varC0,$varC7,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC0;
    } else {
        $varE33 = _varInterp3($varC0,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE02 = $varC0;
        $varE03 = $varC0;
        $varE13 = $varC0;
    } else {
        $varE02 = _varInterp2($varC0,$varC1,1,1);
        $varE03 = _varInterp2($varC1,$varC5,1,1);
        $varE13 = _varInterp2($varC0,$varC5,1,1);
    }
  } elsif (
    ( $intPattern == 0b11111111 ) 
  ){
    $varE01 = $varC4;
    $varE02 = $varC4;
    $varE10 = $varC4;
    $varE11 = $varC4;
    $varE12 = $varC4;
    $varE13 = $varC4;
    $varE20 = $varC4;
    $varE21 = $varC4;
    $varE22 = $varC4;
    $varE23 = $varC4;
    $varE31 = $varC4;
    $varE32 = $varC4;
    if (_boolYUV_NE( $varC7, $varC3 )) {
        $varE30 = $varC4;
    } else {
        $varE30 = _varInterp3($varC4,$varC3,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC7, $varC5 )) {
        $varE33 = $varC4;
    } else {
        $varE33 = _varInterp3($varC4,$varC5,$varC7,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC3 )) {
        $varE00 = $varC4;
    } else {
        $varE00 = _varInterp3($varC4,$varC1,$varC3,2,1,1);
    }
    if (_boolYUV_NE( $varC1, $varC5 )) {
        $varE03 = $varC4;
    } else {
        $varE03 = _varInterp3($varC4,$varC1,$varC5,2,1,1);
    }
  }
  #END LQ4x PATTERNS
  return (
    $varE00, $varE01, $varE02, $varE03, 
    $varE10, $varE11, $varE12, $varE13, 
    $varE20, $varE21, $varE22, $varE23, 
    $varE30, $varE31, $varE32, $varE33, 
  );
}

# standard Scale2x casepath
sub _arrScale2x {
  my (
    $varC0, $varC1, $varC2, 
    $varC3, $varC4, $varC5, 
    $varC6, $varC7, $varC8
  )=@_;
  my ( $varE00, $varE01,  ) = ( $varC4, $varC4, );
  my ( $varE10, $varE11,  ) = ( $varC4, $varC4, );
  #BEGIN Scale2x PATTERNS

    if (_boolYUV_NE( $varC3, $varC5 ) and _boolYUV_NE( $varC1, $varC7 )) {
  if (_boolYUV_E( $varC1, $varC3 )) {
        $varE00=_varInterp2($varC1,$varC3,1,1); 
  };
  if (_boolYUV_E( $varC1, $varC5 )) { 
        $varE01=_varInterp2($varC1,$varC5,1,1);
  };
  if (_boolYUV_E( $varC7, $varC3 )) { 
        $varE10=_varInterp2($varC7,$varC3,1,1); 
  };
  if (_boolYUV_E( $varC7, $varC5 )) { 
        $varE11=_varInterp2($varC7,$varC5,1,1); 
  };
}
  #END Scale2x PATTERNS
  return (
    $varE00, $varE01, 
    $varE10, $varE11, 
  );
}

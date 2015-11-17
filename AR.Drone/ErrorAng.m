function [ErrAng] = ErrorAng(ErrAng)

if ErrAng>=1.1*pi
    while ErrAng>=1.1*pi
    ErrAng=ErrAng-2*pi;
    end
    return
end

if ErrAng<=-1.1*pi
    while ErrAng<=-1.1*pi
    ErrAng=ErrAng+2*pi;
    end
    return
end    
    ErrAng=ErrAng;
return


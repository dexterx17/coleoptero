%ciclo de programacion
clear all
clc
anguloadel=-0.4;
anguloatras=0.4;
anguloder=0.4;
anguloizq=-0.4;
arriba=0;
giroizq=-0.7;
t=0.6;
tg=1.7;
t1=1;
frenar=0;
for j=1:4
    ArDrone([anguloadel 0 0 arriba]);
    tic;
    while (toc<t);
    end
    for frenar=anguloadel:frenar==frenar-0.05:0
        ArDrone([frenar 0 0 arriba]);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
            ArDrone([0 0 giroizq 0]);
            tic;
            while (toc<tg);
            end
            for frenar=giroizq:frenar==frenar+0.05:0
                ArDrone([0 0 frenar arriba]);
            end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
end
ArDrone([0 0 0 0]);
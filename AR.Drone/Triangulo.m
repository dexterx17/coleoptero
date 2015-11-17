%ciclo de programacion
clear all
clc
anguloadel=-0.4;
anguloatras=0.4;
anguloder=0.4;
anguloizq=-0.4;
arriba=0;
giroizq=-0.7;
ta=1.3;
tb=2;
tg1=1.7;
tg2=2.5;
t1=2;
for j=1:1
    ArDrone([anguloadel 0 0 arriba]);
    tic;
    while (toc<ta);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
            ArDrone([0 0 giroizq 0]);
            tic;
            while (toc<tg2);
            end
    ArDrone([anguloadel 0 0 arriba]);
    tic;
    while (toc<tb);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
            ArDrone([0 0 giroizq 0]);
            tic;
            while (toc<tg2);
            end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
    ArDrone([anguloadel 0 0 arriba]);
    tic;
    while (toc<ta);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
            ArDrone([0 0 giroizq 0]);
            tic;
            while (toc<tg1);
            end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
end
ArDrone([0 0 0 0]);
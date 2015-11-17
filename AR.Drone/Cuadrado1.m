%ciclo de programacion
anguloadel=-0.5;
anguloatras=0.5;
anguloder=0.5;
anguloizq=-0.5;
arriba=0;
t=1.2;
t1=1.2;
% adelante pitch 1
% lados roll 2
% giro yaw 3
% altura gaz 4
frenar=0;
for j=1:2
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
    ArDrone([0 anguloizq 0 arriba]);
    tic;
    while (toc<t);
    end
    for frenar=anguloizq:frenar==frenar+0.05:0
        ArDrone([0 frenar 0 arriba]);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
    ArDrone([anguloatras 0 0 arriba]);
    tic;
    while (toc<t);
    end
    for frenar=anguloatras:frenar==frenar+0.05:0
        ArDrone([frenar 0 0 arriba]);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
    ArDrone([0 anguloder 0 arriba]);
    tic;
    while (toc<t);
    end
    for frenar=anguloder:frenar==frenar-0.05:0
        ArDrone([0 frenar 0 arriba]);
    end
        ArDrone([0 0 0 0]);
        tic;
        while (toc<t1);
        end
end
ArDrone([0 0 0 0]);
winopen('Liberar.vbe')
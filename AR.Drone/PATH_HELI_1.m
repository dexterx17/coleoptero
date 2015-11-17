%**************************************************************************
%***********************Seguimiento de CAMINOS*****************************
%*****************************Helic�ptero**********************************
%**************************************************************************

  %POSICION INICIAL DEL ROBOT      
      x_real(1) = 0;               %posici�n en x del Helic�ptero
      y_real(1) = 0;               %posici�n en y del Helic�ptero
      z_real(1) = 0;               %posici�n en z del Helic�ptero
        
      psi(1) = 0*pi/180;           %orientaci�n del Helic�ptero     
      V_DESEADA = 1;             %velocida deseada del Helic�ptero
 
      
%**************************************************************************
%*************************TRAYECTORIAS*************************************
%% **************************************************************************           
   %a) Trayectoria de un c�rculo en xy, un 8 en xz (silla de montar)
 
      ts=.1;  tfin=30; 
       t=[.1:ts:tfin];
       tt=15*t; 
       Pox = 5*cos(0.05*tt)+2;       Pox_p = -5*0.05*sin(0.05*tt);      Pox_2p = -5*0.05*0.05*cos(0.05*tt);
       Poy = 5*sin(0.05*tt)+2;       Poy_p =  5*0.05*cos(0.05*tt);      Poy_2p = -5*0.05*0.05*sin(0.05*tt);
       Poz = 0.5*sin(0.1*tt)+2;      Poz_p =  0.5*0.1*cos(0.1*tt);      Poz_2p = -0.5*0.1*0.1*sin(0.1*tt);
    
       Popsi= atan2(Poy_p,Pox_p);      
       Popsi_p = (1./((Poy_p./Pox_p).^2+1)).*((Poy_2p.*Pox_p-Poy_p.*Pox_2p)./Pox_p.^2);

       FI_yx = atan2(Poy_p,Pox_p);
       FI_xyz = atan2(Poz_p,sqrt(Pox_p.^2+Poy_p.^2));
       
        
   %b) Trayectoria de una espiral en xyz

%        ts=.1;  tfin=30; %500
%       t=[.1:ts:tfin];
%        tt=1*t;
%        Pox = tt/10.*cos(1.25*tt);    Pox_p = 1/10*cos(1.25*tt)-1.25*tt/10.*sin(1.25*tt);    Pox_2p = -1.25/10*sin(1.25*tt)-(1.25/10)*sin(1.25*tt)-(1.25*1.25*tt/10).*cos(1.25*tt);
%        Poy = tt/10.*sin(1.25*tt);    Poy_p = 1/10*sin(1.25*tt)+1.25*tt/10.*cos(1.25*tt);    Poy_2p =  1.25/10*cos(1.25*tt)+(1.25/10)*cos(1.25*tt)-(1.25*1.25*tt/10).*sin(1.25*tt); 
%        Poz = tt/10+3;                Poz_p = 1/10*ones(1,length(tt));                       Poz_2p =  0*ones(1,length(tt)); 
%        
%        Popsi= atan2(Poy_p,Pox_p);      
%        Popsi_p = (1./((Poy_p./Pox_p).^2+1)).*((Poy_2p.*Pox_p-Poy_p.*Pox_2p)./Pox_p.^2);
%        
%        FI_yx = atan2(Poy_p,Pox_p);
%        FI_xyz = atan2(Poz_p,sqrt(Pox_p.^2+Poy_p.^2));

      
   %c) Trayectoria de una senoidal en el eje y
    
%        ts=.1;  tfin=30; %500
%           t=[.1:ts:tfin];
%        tt=20*t;
%        Pox = 0.05*tt+0.8;            Pox_p = 0.05*ones(1,length(tt));    Pox_2p = 0*ones(1,length(tt)); 
%        Poy = 1*sin(0.05*tt)+1;       Poy_p = 1*0.05*cos(0.05*tt);        Poy_2p =-1*0.05*0.05*sin(0.05*tt); 
%        Poz = 0.5*sin(0.05*tt)+1;     Poz_p = 0.5*0.05*cos(0.05*tt);      Poz_2p =-0.5*0.05*0.05*sin(0.05*tt); 
% 
%        Popsi   = atan2(Poy_p,Pox_p);        
%        Popsi_p = (1./((Poy_p./Pox_p).^2+1)).*((Poy_2p.*Pox_p-Poy_p.*Pox_2p)./Pox_p.^2);
%          
%        FI_yx = atan2(Poy_p,Pox_p);
%        FI_xyz = atan2(Poz_p,sqrt(Pox_p.^2+Poy_p.^2));
        
% TU PAPA
    %d) Trayectoria de un cuadrado en xy
% ts=.1;  tfin=40;
% t=[.1:ts:tfin];
%    tt=1*t;    
%     Pox = 10*ones(1,length(tt));           Pox_p = 0*ones(1,length(tt));    Pox_2p = 0*ones(1,length(tt));
%       Poy = 10*ones(1,length(tt));       Poy_p = 0*ones(1,length(tt));        Poy_2p =0*ones(1,length(tt)); 
%        Poz = 3*ones(1,length(tt));        Poz_p = 0*ones(1,length(tt));      Poz_2p =0*ones(1,length(tt));   

% for jjj=1:400
%   if jjj<101
%        Pox(1,jjj)=tt(1,jjj);
%        Pox_p(1,jjj)=-1;
%        Poy_p(1,jjj)=0.05;
%    end
%    if (jjj>100 && jjj<201)
%        Poy(1,jjj)=-1*(tt(1,jjj)-20);
%        Poy_p(1,jjj)=1;
%        Pox_p(1,jjj)=0.05;
%    end       
%     if (jjj>200 && jjj<301)
%        Pox(1,jjj)=-1*(tt(1,jjj)-30);
%        Poy(1,jjj)=0;
%        Pox_p(1,jjj)=1;
%        Poy_p(1,jjj)=0.05;
%    end     
%    if (jjj>300 && jjj<401)
%        Poy(1,jjj)=tt(1,jjj)-30;
%        Pox(1,jjj)=0;
%        Poy_p(1,jjj)=-1;
%        Pox_p(1,jjj)=0.05;
%    end 
%end

%       Popsi   = atan2(Poy_p,Pox_p);        
%        Popsi_p = (1./((Poy_p./Pox_p).^2+1)).*((Poy_2p.*Pox_p-Poy_p.*Pox_2p)./Pox_p.^2);
%          
%        FI_yx = atan2(Poy_p,Pox_p);
%        FI_xyz = atan2(Poz_p,sqrt(Pox_p.^2+Poy_p.^2));
        
%e) Trayectoria de un triangulo en xy
%	ts=.1;  tfin=30; 
%	t=[.1:ts:tfin];
 %   tt=1*t;    
  %  Pox = 0*ones(1,length(tt));           Pox_p = 0*ones(1,length(tt));    Pox_2p = 0*ones(1,length(tt));
%   Poy = 0*ones(1,length(tt));       Poy_p = 0*ones(1,length(tt));        Poy_2p =0*ones(1,length(tt)); 
%    Poz = 3*ones(1,length(tt));        Poz_p = 0*ones(1,length(tt));      Poz_2p =0*ones(1,length(tt));   
%
 %for jjj=1:300
 %   if jjj<101
 %       Pox(1,jjj)=tt(1,jjj);
 %       Pox_p(1,jjj)=-1;
  %      Poy_p(1,jjj)=0.05;
  %  end
  %   if (jjj>100 && jjj<201)
  %      Poy(1,jjj)=1*(tt(1,jjj)-10);
  %      Pox(1,jjj)=10;
  %      Poy_p(1,jjj)=-1;
  %      Pox_p(1,jjj)= 0.05;
  %   end       
  %  if (jjj>200 && jjj<301)
  %     Pox(1,jjj)=-1*(tt(1,jjj)-30);
  %      Poy(1,jjj)=-1*(tt(1,jjj)-30);
  %      Pox_p(1,jjj)=1;
  %      Poy_p(1,jjj)=1;
  %  end     
%end

 %       Popsi   = atan2(Poy_p,Pox_p);        
 %       Popsi_p = (1./((Poy_p./Pox_p).^2+1)).*((Poy_2p.*Pox_p-Poy_p.*Pox_2p)./Pox_p.^2);
          
 %       FI_yx = atan2(Poy_p,Pox_p);
 %       FI_xyz = atan2(Poz_p,sqrt(Pox_p.^2+Poy_p.^2));
        
%POR TU PAPA
%**************************************************************************
%**************************CONTROLADOR*************************************
%% **************************************************************************

 for k=1:length(t)     
%1c) C�lculo del punto m�nimo entre Robot y el camino
    % a) Busqueda del punto m�s cercano del camino
         mini = 100000;    
         for i = 1:length(Pox)
             aux = (x_real(k)-Pox(i))^2+(y_real(k)-Poy(i))^2+(z_real(k)-Poz(i))^2;
             if aux < mini 
             mini = aux;    
             pos = i;
             end
         end
    % b) Datos del punto mas cercano (x,y,z) y los �gulos del camino
      rho(k) = sqrt(mini);      % Distancia m�s corta, helic�ptero-camino
      Fi_YX = FI_yx(pos);       % Angulo deseado en el plano xy
      % F i_YX = 0.65;
        Fi_XYZ = FI_xyz(pos);     % Angulo deseado en el plano zy
        % Fi_XYZ =0.01;

%2) Velocida deseada del helic�ptero sobre el camino a seguir
     if k==1
         V(k) = V_DESEADA;
     else
%              V(k) = V_DESEADA/(1+2*rho(k));            %VELOCIDAD DESEADA cambia deacuerdo al error
           %   V(k) = V_DESEADA/(1+10*abs(w(k-1))^2);  %VELOCIDAD DESEADA cambia deacuerdo a la trayectoria y al error

             V(k) = V_DESEADA; 
     end
    
%3) Error en los ejes: x, y, z
    x_err(k) = Pox(pos)-x_real(k);
    y_err(k) = Poy(pos)-y_real(k);
    z_err(k) = Poz(pos)-z_real(k);
    psi_err(k)= ErrorAng(Popsi(pos)-psi(k));
    
    x_p = 2*tanh(.75*x_err(k)) + V(k)*cos(Fi_YX)*cos(Fi_XYZ);
    y_p = 2*tanh(.75*y_err(k)) + V(k)*sin(Fi_YX)*cos(Fi_XYZ);
    z_p = 2*tanh(.75*z_err(k)) + V(k)*sin(Fi_XYZ);
    psi_p= 5*tanh(.75*psi_err(k))+Popsi_p(k);
    
    XYZ_p = [x_p, y_p, z_p, psi_p]';
    
    V_h(k) = sqrt(x_p^2+y_p^2+z_p^2); 
    
%4) C�LCULO DE LAS ACCIONES DE CONTROL del CONTROLADOR CINEM�TICO     
    % a) Matriz del helic�ptero    
         J = [[cos(psi(k)) -sin(psi(k))  0  0];...
              [sin(psi(k))  cos(psi(k))  0  0];...
              [        0             0   1  0];...
              [        0             0   0  1]];

    % b) C�lculo de control basado en la Imagen
         solucion = inv(J)*(XYZ_p);
          
    % c) Separa las acciones de control, para el movil necesito el m�dulo de la velocidad lineal           
         vl(k) = solucion(1);         %en m/s
         vm(k) = solucion(2);         %en m/s
         vn(k) = solucion(3);         %en m/s
          w(k) = solucion(4);         %en rad/s
        
%6) APLICAR LAS ACCIONES DE CONTROL AL HELICOPTERO (por el momento el )
    % a) Al modelo cinem�tico del Helicoptero      
         x_real(k+1) = x_real(k)+ts*(vl(k)*cos(psi(k))-vm(k)*sin(psi(k)));
         y_real(k+1) = y_real(k)+ts*(vl(k)*sin(psi(k))+vm(k)*cos(psi(k)));
         z_real(k+1) = z_real(k)+vn(k)*ts;
         psi(k+1) = ErrorAng(psi(k)+w(k)*ts);      
 end

%**************************************************************************
%****************************ANIMACI�N*************************************
%% **************************************************************************   
        i=1;  paso=1; fig=figure; %290
        set(fig,'position',[100 60 980 600]); 
        axis equal;  cameratoolbar
        
        Helicopter_Parameters;
        H1 = Helicopter_Plot_3D([x_real(1),y_real(1),z_real(1),0,0,psi(1)]);hold on
        
        H2 = plot3(Pox(1),Poy(1),Poz(1),'g','MarkerSize',20); 
        H3 = plot3(x_real(1),y_real(1),z_real(1),'m'); 
        
%     axis([-1 15 -2 6]);    
    for nn=i:paso:length(t) 
        drawnow 
        delete(H1);
        delete(H2);
        delete(H3);
        
        H1 = Helicopter_Plot_3D([x_real(nn),y_real(nn),z_real(nn),0,0,psi(nn)]); hold on   
        H2 = plot3(Pox,Poy,Poz,'g'); hold on

        if nn>200
            H3=plot3(x_real(nn-199:nn),y_real(nn-199:nn),z_real(nn-199:nn),'m');   %es la trayectoria que hace        
        else
            H3=plot3(x_real(1:nn),y_real(1:nn),z_real(1:nn),'m');   %es la trayectoria que hace       
        end
    end
    H1=Helicopter_Plot_3D([x_real(1),y_real(1),z_real(1),0,0,psi(1)]);hold on        
    plot3(x_real,y_real,z_real,'r','LineWidth',1); hold on;

%**************************************************************************
%***************************GR�FICAS***************************************
%% **************************************************************************

figure(2);
        plot(t,rho,'g'); grid
        xlabel('Time [s]'), ylabel('[m]')
        title('\rho [m]'); 

figure(3);
            plot3(x_real,y_real,z_real,'r','LineWidth',2); hold on;
            plot3(Pox,Poy,Poz,'g','LineWidth',1); grid 
        xlabel('x [m]'), ylabel('y [m]'), zlabel('z [m]')
%         legend('Real','Deseada')         

        
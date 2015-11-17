#include <windows.h>
#include <math.h>
#include "mex.h"

/* Input Arguments */

#define	Y_IN	prhs[0]

/* output Arguments*/

#define	YP_OUT	plhs[0]

#if !defined(max)
#define	max(A, B)	((A) > (B) ? (A) : (B))
#endif

#if !defined(min)
#define	min(A, B)	((A) < (B) ? (A) : (B))
#endif

///ESTRUCTURA QUE FORMA LA MEMORIA VIRTUAL//////
#define M_COMPARTIDA  "SIM_ARDRONE"
#define Nd	2
#define Md	18

typedef struct tag_VIRTUAL_DEVICE{

	float dispositivo[Nd][Md];

	//dispositivo[0][0] -> Battery level [1 -> 100%]
	//dispositivo[0][1] -> Theta en rad (pitch)
	//dispositivo[0][2] -> Phi en rad (roll)
	//dispositivo[0][3] -> Psi en rad (yaw)
	//dispositivo[0][4] -> Altitud en metros
	//dispositivo[0][5] -> Velocidad eje X (forward) en m/s
	//dispositivo[0][6] -> Velocidad eje Y (sideward) en m/s
	//dispositivo[0][7] -> Velocidad eje Z en m/s
	//dispositivo[0][8] -> Giróscopo[0] (no se si es º/s o rad/s)
	//dispositivo[0][9] -> Giróscopo[1] (no se si es º/s o rad/s)
	//dispositivo[0][10] -> Giróscopo[2] (no se si es º/s o rad/s)
	//dispositivo[0][11] -> Acelerómetro[0] (en m/s2, creo)
	//dispositivo[0][12] -> Acelerómetro[1] (en m/s2, creo)
	//dispositivo[0][13] -> Acelerómetro[2] (en m/s2, creo)

	//LUCIO PRUEBA PARA LEER PWM EN MATLAB//09/03/2012
	//dispositivo[0][14] -> PWM Motor 1 [0 -> 255]
	//dispositivo[0][15] -> PWM Motor 2 [0 -> 255]
	//dispositivo[0][16] -> PWM Motor 3 [0 -> 255]
	//dispositivo[0][17] -> PWM Motor 4 [0 -> 255]
	//LUCIO PRUEBA PARA LEER PWM EN MATLAB//09/03/2012

	//dispositivo[1][0] -> comando pitch (desde Matlab, entre -1 y 1, +-12 grados)
	//dispositivo[1][1] -> comando roll (desde Matlab, entre -1 y 1, +-12 grados)
	//dispositivo[1][2] -> comando yaw (desde Matlab, entre -1 y 1, +-100 grados/segundo)
	//dispositivo[1][3] -> comando gaz (desde Matlab, entre -1 y 1, +- 0.7 m/s)


	//LUCIO PRUEBA PARA MANDAR LA IMAGEN A MATLAB//26/10/2011
	//unsigned char imagen_bits[3][144][176];
	unsigned char imagen_chica_bits[3][144][176];
	unsigned char imagen_grande_bits[3][240][320];
	BOOL imagen_grande;
	//array_name[table][row][column]
	//ArDrone-> array_name={r11,g11,b11,r12,g12,b12,r...}
	//Matlab -> CData -> table 0=red, table 1=green, table 2=blue
	//LUCIO PRUEBA PARA MANDAR LA IMAGEN A MATLAB//26/10/2011

} VIRTUAL_DEVICE;
///ESTRUCTURAS QUE FORMAN LA MEMORIA VIRTUAL//////

///VARIABLES GLOBALES/////////
VIRTUAL_DEVICE *CrearDatos(char *cpNombre,HANDLE *phMem);
VIRTUAL_DEVICE *pDrone;
HANDLE hDrone;
///VARIABLES GLOBALES/////////

//FUNCION QUE CREA LA MEMORIA VIRTUAL Y DEVUELVE UN PUNTERO A LA MSIMA////
VIRTUAL_DEVICE *CrearDatos(char *cpNombre,HANDLE *phMem)
{
HANDLE hMemShared;
VIRTUAL_DEVICE *pDatos;

	hMemShared = CreateFileMapping((HANDLE)NULL,NULL,PAGE_READWRITE  ,0
									,sizeof(VIRTUAL_DEVICE),cpNombre);
  	if(hMemShared == NULL){
		mexErrMsgTxt("Couldn´t create shared memory\n");
		return NULL;
	}
    
	pDatos = (VIRTUAL_DEVICE *)MapViewOfFile(hMemShared,FILE_MAP_ALL_ACCESS,0,0,sizeof(VIRTUAL_DEVICE));
    
	if(pDatos == NULL){
		mexErrMsgTxt("Couldn´t create view of shared memory\n");
		return NULL;
	}
	*phMem = hMemShared;
	return pDatos;
}
//FUNCION QUE CREA LA MEMORIA VIRTUAL Y DEVUELVE UN PUNTERO A LA MSIMA////

///FUNCION QUE ESCRIBE EN MEMORIA VIRTUAL////////
BOOL FlushDatos(VIRTUAL_DEVICE *pDatos)
{
	FlushViewOfFile(pDatos ,sizeof(VIRTUAL_DEVICE));
	return TRUE;
}
///FUNCION QUE ESCRIBE EN MEMORIA VIRTUAL////////

///FUNCION QUE CIERRA LA MEMORIA VIRTUAL////////
BOOL CerrarDatos(VIRTUAL_DEVICE *pDatos)
{
	FlushViewOfFile(pDatos ,sizeof(VIRTUAL_DEVICE));
    UnmapViewOfFile(pDatos);
    CloseHandle(hDrone);
    pDatos=NULL;
	return TRUE;
}       
///FUNCION QUE CIERRA LA MEMORIA VIRTUAL////////

////FUNCION LLAMADA POR MATLAB (en este caso "ArDrone")/////
void mexFunction(int nlhs, mxArray *plhs[], int nrhs, const mxArray *prhs[])
{
	double	*u;
	double	*yp;
	unsigned int	m,n;
	int k;
	
	/* Check for proper number of arguments */
	  
	if (nrhs == 1) //pregunta si hay un solo argumento de entrada
	{
		/* Check the dimensions of Y.  Y must be 1 X 4. */
  		m = mxGetM(Y_IN);
		n = mxGetN(Y_IN);
		if (!mxIsNumeric(Y_IN) || mxIsComplex(Y_IN) || 
			(max(m,n) != 4) || (min(m,n) != 1))
		{
			mexErrMsgTxt("Se requiere que u sea un vector de 1 x 4 o 4 x 1");
		}
	      
		/* Assign pointers to the various parameters */
		u = mxGetPr(Y_IN);
	  
		pDrone = CrearDatos(M_COMPARTIDA,&hDrone);
		if(pDrone == NULL)
		{
			mexErrMsgTxt("Error CrearDatos() Drone\n");
			return;
		}
		/* Do the computations */
        for(k=0;k<4;k++)
				pDrone->dispositivo[1][k] = u[k];
              
		FlushDatos(pDrone);//Actualiza los datos en la memoria virtual
	}
	else if (nrhs > 1)//pregunta si hay mas de un argumento de entrada
	{
		mexErrMsgTxt("Se requiere un sólo argumento u=[pitch roll yaw gaz]");
	}
  		
	/* Check for proper number of arguments */
  
	if (nrhs == 0)//pregunta si no hay ningun argumento de entrada
	{
		if (nlhs > 1)//pregunta si hay mas de un argumento de salida
		{
			mexErrMsgTxt("Se requiere un sólo argumento de salida");
		}
		else
		{
			/* Create a matrix for the return argument */
	  
			YP_OUT = mxCreateDoubleMatrix(18, 1, mxREAL);
	  
			/* Assign pointers to the various parameters */
	  
			yp = mxGetPr(YP_OUT);
	    
			pDrone = CrearDatos(M_COMPARTIDA,&hDrone);
			if(pDrone == NULL)
			{
				mexErrMsgTxt("Error CrearDatos() Drone\n");
				return;
			}
	  
			/* Do the computations*/
			for(k=0;k<18;k++)
				yp[k] = pDrone->dispositivo[0][k];
		}
        CerrarDatos(pDrone);//Cierra la memoria virtual
		return;
	}
    CerrarDatos(pDrone);//Cierra la memoria virtual
	return;
}
////FUNCION LLAMADA POR MATLAB (en este caso "ArDrone")/////



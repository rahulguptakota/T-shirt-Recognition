#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2\core\core.hpp"
#include "opencv2\features2d\features2d.hpp"
#include "opencv2\photo\photo.hpp"
#include <stdlib.h>
#include <stdio.h>
#include <fstream>
#include <iostream>
#include <Windows.h>

using namespace cv;
using namespace std;

/// Global variables
string location = "C:\\tshirt recognition\\";



Scalar getMSSIM(const Mat& i1, const Mat& i2)
{
	const double C1 = 6.5025, C2 = 58.5225;
	/***************************** INITS **********************************/
	int d = CV_32F;

	Mat I1, I2;
	i1.convertTo(I1, d);           // cannot calculate on one byte large values
	i2.convertTo(I2, d);

	Mat I2_2 = I2.mul(I2);        // I2^2
	Mat I1_2 = I1.mul(I1);        // I1^2
	Mat I1_I2 = I1.mul(I2);        // I1 * I2

	/*************************** END INITS **********************************/

	Mat mu1, mu2;   // PRELIMINARY COMPUTING
	GaussianBlur(I1, mu1, Size(11, 11), 1.5);
	GaussianBlur(I2, mu2, Size(11, 11), 1.5);

	Mat mu1_2 = mu1.mul(mu1);
	Mat mu2_2 = mu2.mul(mu2);
	Mat mu1_mu2 = mu1.mul(mu2);

	Mat sigma1_2, sigma2_2, sigma12;

	GaussianBlur(I1_2, sigma1_2, Size(11, 11), 1.5);
	sigma1_2 -= mu1_2;

	GaussianBlur(I2_2, sigma2_2, Size(11, 11), 1.5);
	sigma2_2 -= mu2_2;

	GaussianBlur(I1_I2, sigma12, Size(11, 11), 1.5);
	sigma12 -= mu1_mu2;

	///////////////////////////////// FORMULA ////////////////////////////////
	Mat t1, t2, t3;

	t1 = 2 * mu1_mu2 + C1;
	t2 = 2 * sigma12 + C2;
	t3 = t1.mul(t2);              // t3 = ((2*mu1_mu2 + C1).*(2*sigma12 + C2))

	t1 = mu1_2 + mu2_2 + C1;
	t2 = sigma1_2 + sigma2_2 + C2;
	t1 = t1.mul(t2);               // t1 =((mu1_2 + mu2_2 + C1).*(sigma1_2 + sigma2_2 + C2))

	Mat ssim_map;
	divide(t3, t1, ssim_map);      // ssim_map =  t3./t1;

	Scalar mssim = mean(ssim_map); // mssim = average of ssim map
	return mssim;
}

int APIENTRY WinMain(HINSTANCE hInstance,HINSTANCE hPrevInstance,LPSTR lpCmdLine,int nCmdShow)
{
	Mat im1, im2;

	ifstream file;
	file.open(location + "i1.txt");
	string f1;
	getline(file, f1);
	file.clear();
	file.close();

	ifstream file1;
	file1.open(location + "i2.txt");
	string f2;
	getline(file1, f2);
	file1.clear();
	file1.close();

	im1 = imread(f1);
	im2 = imread(f2);

	if (!im1.data)
	{
		return -1;
	}
	if (!im2.data)
	{
		return -1;
	}

	int row1, col1, row2, col2;
	row1 = im1.rows;
	row2 = im2.rows;
	col1 = im1.cols;
	col2 = im2.cols;
	Rect r1((col1 / 4), (row1 / 5), (col1 / 2), (4 * row1 / 7));
	Rect r2((col2 / 4), (row2 / 5), (col2 / 2), (4 * row2 / 7));
	im1 = im1(r1);
	im2 = im2(r2);

	Scalar ssim = getMSSIM(im1, im2);
	double r, g, b;
	b = ssim[0];
	g = ssim[1];
	r = ssim[2];
	//cout << b << endl << g << endl << r<<endl<<(b*g*r);
	//getchar();
	double rate = b*g*r;
	std::ostringstream strs;
	strs << rate;
	string str1 = strs.str();
	int l = str1.length();
	char a[100];
	for (int i = 0; i < l; i++)
		*(a + i) = str1[i];
	ofstream myfile;
	myfile.open(location + "Rate.txt");
	myfile.write(a, l);
	myfile.close();
}
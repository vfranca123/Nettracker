using Microsoft.AspNetCore.Mvc;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace Api.controller
{
    [ApiController] //diz que essa classe é um atendente da api 
    [Route("api/controller")]//o caminho que o navegador segue ate o atendente
    public class RedeController : ControllerBase // define que a rede controller extendera o controller base 
    {
        [HttpPost] // Responde quando alguem envia dados
        public IActionResult PostRede([FromBodyAttribute] Rede rede)
        { // uma função do tipo  "IActionResult" que recebera os atributos do corpo e tentara colocaro em um objeto com nome "rede" da classe Rede que esta em models 
          // onde a logica é desenvolvida 


            return Ok(new
            {
                mensagem = "dados recebidos",
                dados = rede
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API get está funcionando!");
        }


        public void returnIpsSubredes(Rede rede)
        {


        }

        //dependencias pra retornar as subredes
        static bool isThisNetworkValid(String ipRede) //o out em c# permite que a função retorne mais de um valor "entender melhor " 
        {
            var partes = ipRede.Split('/');

            if (partes.Length != 2) return false;
            return true;
                
        }
    }
  
}